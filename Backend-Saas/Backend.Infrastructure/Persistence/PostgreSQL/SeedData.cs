using Backend.Domain.Common;
using Backend.Domain.Saas.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Persistence.PostgreSQL;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var db = serviceProvider.GetRequiredService<AppDbContext>();

        await EnsureRoleAsync(roleManager, "SuperAdmin", "Super administrador global del sistema");
        await EnsureRoleAsync(roleManager, "AdminTenant", "Administrador de un tenant");
        await EnsureRoleAsync(roleManager, "Usuario", "Usuario regular del sistema");

        var superAdmin = await userManager.FindByEmailAsync("admin@saas.com");
        if (superAdmin is null)
        {
            superAdmin = new ApplicationUser
            {
                UserName = "admin@saas.com",
                Email = "admin@saas.com",
                FirstName = "Super",
                LastName = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(superAdmin, "Admin123!");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
        }

        await SeedSaasCatalogAsync(db);
    }

    private static async Task EnsureRoleAsync(RoleManager<ApplicationRole> roleManager, string name, string description)
    {
        if (await roleManager.RoleExistsAsync(name))
            return;

        await roleManager.CreateAsync(new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        });
    }

    private static async Task SeedSaasCatalogAsync(AppDbContext db)
    {
        var definitions = BuildCatalogDefinitions();
        var systemsByKey = await UpsertSystemsAsync(db, definitions);
        var modulesBySystemAndKey = await UpsertModulesAsync(db, definitions, systemsByKey);
        var subModules = await UpsertSubModulesAsync(db, definitions, modulesBySystemAndKey);

        await UpsertPlansAsync(db, definitions, systemsByKey, modulesBySystemAndKey, subModules);
        await UpsertPermissionsAsync(db, systemsByKey, modulesBySystemAndKey, subModules);

        await db.SaveChangesAsync();
    }

    private static async Task<Dictionary<string, SaasSystem>> UpsertSystemsAsync(AppDbContext db, IReadOnlyCollection<SystemSeed> definitions)
    {
        var existingSystems = await db.SaasSystems.ToDictionaryAsync(s => s.Key);
        var systemsByKey = new Dictionary<string, SaasSystem>(StringComparer.OrdinalIgnoreCase);

        foreach (var definition in definitions)
        {
            if (!existingSystems.TryGetValue(definition.Key, out var system))
            {
                system = new SaasSystem { Id = Guid.NewGuid(), Key = definition.Key };
                db.SaasSystems.Add(system);
            }

            system.Name = definition.Name;
            system.Description = definition.Description;
            system.Icon = definition.Icon;
            system.BasePath = definition.BasePath;
            system.DisplayOrder = definition.DisplayOrder;
            system.IsActive = true;
            system.UpdatedAt = DateTime.UtcNow;
            systemsByKey[definition.Key] = system;
        }

        await db.SaveChangesAsync();
        return systemsByKey;
    }

    private static async Task<Dictionary<string, SaasModule>> UpsertModulesAsync(
        AppDbContext db,
        IReadOnlyCollection<SystemSeed> definitions,
        IReadOnlyDictionary<string, SaasSystem> systemsByKey)
    {
        var existingModules = await db.SaasModules.ToListAsync();
        var modulesBySystemAndKey = new Dictionary<string, SaasModule>(StringComparer.OrdinalIgnoreCase);

        foreach (var systemDefinition in definitions)
        {
            var system = systemsByKey[systemDefinition.Key];

            foreach (var moduleDefinition in systemDefinition.Modules)
            {
                var module = existingModules.FirstOrDefault(m => m.SystemId == system.Id && m.Key == moduleDefinition.Key);
                if (module is null)
                {
                    module = new SaasModule { Id = Guid.NewGuid(), SystemId = system.Id, Key = moduleDefinition.Key };
                    db.SaasModules.Add(module);
                    existingModules.Add(module);
                }

                module.Name = moduleDefinition.Name;
                module.Description = moduleDefinition.Description;
                module.Icon = moduleDefinition.Icon;
                module.BasePath = moduleDefinition.BasePath;
                module.DisplayOrder = moduleDefinition.DisplayOrder;
                module.IsActive = true;
                module.UpdatedAt = DateTime.UtcNow;
                modulesBySystemAndKey[MapKey(systemDefinition.Key, moduleDefinition.Key)] = module;
            }
        }

        await db.SaveChangesAsync();
        return modulesBySystemAndKey;
    }

    private static async Task<List<SubModuleSeedResult>> UpsertSubModulesAsync(
        AppDbContext db,
        IReadOnlyCollection<SystemSeed> definitions,
        IReadOnlyDictionary<string, SaasModule> modulesBySystemAndKey)
    {
        var existingSubModules = await db.SaasSubModules.ToListAsync();
        var results = new List<SubModuleSeedResult>();

        foreach (var systemDefinition in definitions)
        {
            foreach (var moduleDefinition in systemDefinition.Modules)
            {
                var module = modulesBySystemAndKey[MapKey(systemDefinition.Key, moduleDefinition.Key)];

                foreach (var subModuleDefinition in moduleDefinition.SubModules)
                {
                    var subModule = existingSubModules.FirstOrDefault(sm => sm.ModuleId == module.Id && sm.Key == subModuleDefinition.Key);
                    if (subModule is null)
                    {
                        subModule = new SaasSubModule { Id = Guid.NewGuid(), ModuleId = module.Id, Key = subModuleDefinition.Key };
                        db.SaasSubModules.Add(subModule);
                        existingSubModules.Add(subModule);
                    }

                    subModule.Name = subModuleDefinition.Name;
                    subModule.Description = subModuleDefinition.Description;
                    subModule.RoutePath = subModuleDefinition.RoutePath;
                    subModule.DisplayOrder = subModuleDefinition.DisplayOrder;
                    subModule.IsActive = true;
                    subModule.UpdatedAt = DateTime.UtcNow;

                    results.Add(new SubModuleSeedResult(systemDefinition.Key, moduleDefinition.Key, subModule));
                }
            }
        }

        await db.SaveChangesAsync();
        return results;
    }

    private static async Task UpsertPlansAsync(
        AppDbContext db,
        IReadOnlyCollection<SystemSeed> definitions,
        IReadOnlyDictionary<string, SaasSystem> systemsByKey,
        IReadOnlyDictionary<string, SaasModule> modulesBySystemAndKey,
        IReadOnlyCollection<SubModuleSeedResult> subModules)
    {
        var starter = await UpsertPlanAsync(db, "starter", "Starter", "Plan inicial para operar SaaS Central, ERP, POS y CRM", 99, 5, 1, 500);
        var professional = await UpsertPlanAsync(db, "professional", "Professional", "Plan operativo con sistemas omnicanal y logísticos", 299, 25, 5, 5000);
        var enterprise = await UpsertPlanAsync(db, "enterprise", "Enterprise", "Plan completo con inteligencia, prevención y BI", 799, 250, 50, 100000);

        var starterSystems = new[] { "saas-central", "erp", "pos", "crm" };
        var professionalSystems = new[] { "saas-central", "erp", "pos", "crm", "rh", "oms", "wms", "tms", "pim", "sfa", "help-desk" };
        var enterpriseSystems = definitions.Select(s => s.Key).ToArray();

        await IncludePlanScopeAsync(db, starter, starterSystems, systemsByKey, modulesBySystemAndKey, subModules);
        await IncludePlanScopeAsync(db, professional, professionalSystems, systemsByKey, modulesBySystemAndKey, subModules);
        await IncludePlanScopeAsync(db, enterprise, enterpriseSystems, systemsByKey, modulesBySystemAndKey, subModules);
    }

    private static async Task<SaasPlan> UpsertPlanAsync(
        AppDbContext db,
        string key,
        string name,
        string description,
        decimal price,
        int maxUsers,
        int maxBranches,
        int maxDocumentsPerMonth)
    {
        var plan = await db.SaasPlans.FirstOrDefaultAsync(p => p.Key == key);
        if (plan is null)
        {
            plan = new SaasPlan { Id = Guid.NewGuid(), Key = key };
            db.SaasPlans.Add(plan);
        }

        plan.Name = name;
        plan.Description = description;
        plan.Price = price;
        plan.Currency = "PEN";
        plan.BillingCycle = "Monthly";
        plan.MaxUsers = maxUsers;
        plan.MaxBranches = maxBranches;
        plan.MaxDocumentsPerMonth = maxDocumentsPerMonth;
        plan.IsActive = true;
        plan.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return plan;
    }

    private static async Task IncludePlanScopeAsync(
        AppDbContext db,
        SaasPlan plan,
        IReadOnlyCollection<string> systemKeys,
        IReadOnlyDictionary<string, SaasSystem> systemsByKey,
        IReadOnlyDictionary<string, SaasModule> modulesBySystemAndKey,
        IReadOnlyCollection<SubModuleSeedResult> subModules)
    {
        foreach (var systemKey in systemKeys)
        {
            if (!systemsByKey.TryGetValue(systemKey, out var system))
                continue;

            if (!await db.PlanSystems.AnyAsync(ps => ps.PlanId == plan.Id && ps.SystemId == system.Id))
                db.PlanSystems.Add(new PlanSystem { PlanId = plan.Id, SystemId = system.Id });
        }

        foreach (var moduleEntry in modulesBySystemAndKey.Where(m => systemKeys.Contains(m.Key.Split('.')[0])))
        {
            if (!await db.PlanModules.AnyAsync(pm => pm.PlanId == plan.Id && pm.ModuleId == moduleEntry.Value.Id))
                db.PlanModules.Add(new PlanModule { PlanId = plan.Id, ModuleId = moduleEntry.Value.Id });
        }

        foreach (var subModule in subModules.Where(sm => systemKeys.Contains(sm.SystemKey)))
        {
            if (!await db.PlanSubModules.AnyAsync(psm => psm.PlanId == plan.Id && psm.SubModuleId == subModule.SubModule.Id))
                db.PlanSubModules.Add(new PlanSubModule { PlanId = plan.Id, SubModuleId = subModule.SubModule.Id });
        }

        await db.SaveChangesAsync();
    }

    private static async Task UpsertPermissionsAsync(
        AppDbContext db,
        IReadOnlyDictionary<string, SaasSystem> systemsByKey,
        IReadOnlyDictionary<string, SaasModule> modulesBySystemAndKey,
        IReadOnlyCollection<SubModuleSeedResult> subModules)
    {
        var existingKeys = await db.Permissions.Select(p => p.Key).ToListAsync();
        var existingKeySet = existingKeys.ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var subModule in subModules)
        {
            var system = systemsByKey[subModule.SystemKey];
            var module = modulesBySystemAndKey[MapKey(subModule.SystemKey, subModule.ModuleKey)];
            var prefix = $"{subModule.SystemKey}.{subModule.ModuleKey}.{subModule.SubModule.Key}";

            AddPermissionIfMissing(db, existingKeySet, system, module, subModule.SubModule, $"{prefix}.ver", "view", $"Ver {subModule.SubModule.Name}");
            AddPermissionIfMissing(db, existingKeySet, system, module, subModule.SubModule, $"{prefix}.administrar", "manage", $"Administrar {subModule.SubModule.Name}");
        }
    }

    private static void AddPermissionIfMissing(
        AppDbContext db,
        ISet<string> existingKeySet,
        SaasSystem system,
        SaasModule module,
        SaasSubModule subModule,
        string key,
        string action,
        string name)
    {
        if (existingKeySet.Contains(key))
            return;

        db.Permissions.Add(new Permission
        {
            Id = Guid.NewGuid(),
            SaasSystemId = system.Id,
            SaasModuleId = module.Id,
            SaasSubModuleId = subModule.Id,
            Name = name,
            Key = key,
            Action = action,
            Description = $"Permite {name.ToLowerInvariant()}"
        });

        existingKeySet.Add(key);
    }

    private static IReadOnlyCollection<SystemSeed> BuildCatalogDefinitions()
    {
        var systems = new List<SystemSeed>();

        SystemSeed AddSystem(string key, string name, string description, string icon, string basePath, int order)
        {
            var system = new SystemSeed(key, name, description, icon, basePath, order, new List<ModuleSeed>());
            systems.Add(system);
            return system;
        }

        ModuleSeed AddModule(SystemSeed system, string key, string name, string description, string icon, string basePath, int order)
        {
            var module = new ModuleSeed(key, name, description, icon, basePath, order, new List<SubModuleSeed>());
            system.Modules.Add(module);
            return module;
        }

        void AddSub(ModuleSeed module, string key, string name, string description, string routePath, int order)
        {
            module.SubModules.Add(new SubModuleSeed(key, name, description, routePath, order));
        }

        var saas = AddSystem("saas-central", "Plataforma SaaS Central", "Venta, control, activación, facturación y gobierno del producto SaaS", "layout-dashboard", "/superadmin", 0);
        var catalogo = AddModule(saas, "catalogo-comercial", "Catálogo Comercial de Sistemas", "Sistemas, módulos, submódulos, versiones, paquetes y disponibilidad", "boxes", "/superadmin/saas-catalog", 1);
        AddSub(catalogo, "sistemas", "Sistemas vendibles", "Registro de sistemas disponibles", "/superadmin/saas-catalog/systems", 1);
        AddSub(catalogo, "modulos", "Módulos vendibles", "Registro de módulos por sistema", "/superadmin/saas-catalog/modules", 2);
        AddSub(catalogo, "submodulos", "Submódulos vendibles", "Registro de funciones activables por plan o add-on", "/superadmin/saas-catalog/submodules", 3);
        AddSub(catalogo, "paquetes", "Paquetes comerciales", "Starter, Professional, Enterprise y planes personalizados", "/superadmin/saas-catalog/packages", 4);

        var tenants = AddModule(saas, "clientes-tenants", "Clientes, Tenants y Empresas", "Alta de clientes, empresas, sucursales y configuración inicial", "building-2", "/superadmin/tenants", 2);
        AddSub(tenants, "alta-tenants", "Alta de clientes SaaS", "Creación y administración de tenants", "/superadmin/tenants", 1);
        AddSub(tenants, "empresas-sucursales", "Empresas y sucursales", "Empresas, sucursales y unidades de negocio por cliente", "/superadmin/tenants/branches", 2);
        AddSub(tenants, "aislamiento-datos", "Aislamiento de datos", "Configuración y control de aislamiento por tenant", "/superadmin/tenants/isolation", 3);
        AddSub(tenants, "configuracion-inicial", "Configuración inicial", "Rubro, país, moneda e impuestos", "/superadmin/tenants/setup", 4);

        var suscripciones = AddModule(saas, "suscripciones-licencias", "Suscripciones, Planes y Licencias", "Contratación, límites, vigencias, pruebas, upgrades y add-ons", "badge-check", "/superadmin/subscriptions", 3);
        AddSub(suscripciones, "contratacion", "Contratación", "Contratación de sistemas, módulos y submódulos", "/superadmin/subscriptions/contracts", 1);
        AddSub(suscripciones, "licencias", "Licencias por empresa", "Sistemas, módulos y submódulos habilitados por tenant", "/superadmin/subscriptions/licenses", 2);
        AddSub(suscripciones, "limites-usuarios", "Usuarios y límites", "Usuarios incluidos, adicionales y límites de uso", "/superadmin/subscriptions/limits", 3);
        AddSub(suscripciones, "ciclos-suscripcion", "Ciclos de suscripción", "Inicio, renovación, suspensión y cancelación", "/superadmin/subscriptions/cycles", 4);
        AddSub(suscripciones, "planes", "Planes", "Starter, Professional, Enterprise y planes personalizados", "/superadmin/subscriptions/plans", 5);

        var cobranza = AddModule(saas, "facturacion-cobranza", "Facturación SaaS y Cobranza", "Facturación recurrente, pagos, estados, descuentos y notas de crédito", "credit-card", "/superadmin/billing", 4);
        AddSub(cobranza, "facturacion-recurrente", "Facturación recurrente", "Facturación mensual y anual", "/superadmin/billing/recurring", 1);
        AddSub(cobranza, "pasarelas-pago", "Pasarelas de pago", "Integración con proveedores de pago", "/superadmin/billing/gateways", 2);
        AddSub(cobranza, "estados-pago", "Estados de pago", "Pendiente, pagado, vencido y suspendido", "/superadmin/billing/payment-status", 3);
        AddSub(cobranza, "promociones", "Promociones y notas", "Cupones, descuentos, promociones y notas de crédito", "/superadmin/billing/promotions", 4);

        var provisioning = AddModule(saas, "aprovisionamiento", "Aprovisionamiento y Activación", "Creación automática del entorno y activación granular", "rocket", "/superadmin/provisioning", 5);
        AddSub(provisioning, "entorno-cliente", "Entorno del cliente", "Creación automática del entorno del cliente", "/superadmin/provisioning/environments", 1);
        AddSub(provisioning, "activacion-sistemas", "Activación de sistemas", "Activación y desactivación de sistemas por suscripción", "/superadmin/provisioning/systems", 2);
        AddSub(provisioning, "activacion-granular", "Activación granular", "Activación de módulos y submódulos", "/superadmin/provisioning/modules", 3);
        AddSub(provisioning, "plantillas-configuracion", "Plantillas", "Plantillas de configuración inicial por tipo de cliente", "/superadmin/provisioning/templates", 4);

        var seguridad = AddModule(saas, "seguridad-global", "Seguridad, Roles y Permisos Globales", "Administradores, roles globales, permisos y auditoría", "shield-check", "/superadmin/security", 6);
        AddSub(seguridad, "usuarios-admin", "Usuarios administradores", "Administradores globales del SaaS", "/superadmin/security/admin-users", 1);
        AddSub(seguridad, "roles-globales", "Roles globales", "Super Admin, Soporte, Ventas, Finanzas e Implementación", "/superadmin/security/roles", 2);
        AddSub(seguridad, "permisos", "Permisos", "Permisos por sistema, módulo, submódulo y acción", "/superadmin/security/permissions", 3);
        AddSub(seguridad, "auditoria", "Auditoría", "Accesos, cambios de planes y cambios de configuración", "/superadmin/security/audit", 4);

        var monitoreo = AddModule(saas, "monitoreo-uso", "Monitoreo, Uso y Límites", "Métricas, límites, alertas y salud del servicio", "activity", "/superadmin/monitoring", 7);
        AddSub(monitoreo, "metricas-uso", "Métricas de uso", "Uso por tenant, sistema y módulo", "/superadmin/monitoring/usage", 1);
        AddSub(monitoreo, "limites-plan", "Límites por plan", "Usuarios, sucursales, documentos, API calls y almacenamiento", "/superadmin/monitoring/limits", 2);
        AddSub(monitoreo, "alertas-consumo", "Alertas de consumo", "Alertas de consumo, abuso o vencimiento", "/superadmin/monitoring/alerts", 3);
        AddSub(monitoreo, "salud-servicio", "Salud del servicio", "Panel de salud por cliente", "/superadmin/monitoring/health", 4);

        var backoffice = AddModule(saas, "backoffice-soporte", "Backoffice Comercial y Soporte SaaS", "Leads, demos, onboarding, soporte e historial del cliente", "headphones", "/superadmin/backoffice", 8);
        AddSub(backoffice, "leads-demos", "Leads y demos", "Gestión de leads, demos y oportunidades SaaS", "/superadmin/backoffice/leads", 1);
        AddSub(backoffice, "onboarding", "Onboarding", "Implementación de nuevos clientes", "/superadmin/backoffice/onboarding", 2);
        AddSub(backoffice, "tickets-internos", "Tickets internos", "Soporte interno por tenant", "/superadmin/backoffice/tickets", 3);
        AddSub(backoffice, "historial-cliente", "Historial del cliente", "Historial comercial, contractual y operativo", "/superadmin/backoffice/history", 4);

        var erp = AddSystem("erp", "ERP", "Núcleo financiero, contable, compras, inventario y facturación", "landmark", "/erp", 1);
        var erpFinanzas = AddModule(erp, "finanzas-contabilidad", "Finanzas y Contabilidad", "Contabilidad, cuentas, caja y resultados", "chart-no-axes-combined", "/erp/finanzas-contabilidad", 1);
        AddSub(erpFinanzas, "contabilidad-general", "Contabilidad general", "Libro diario y mayor", "/erp/finanzas-contabilidad/contabilidad-general", 1);
        AddSub(erpFinanzas, "cuentas-cobrar-pagar", "Cuentas por cobrar y pagar", "Gestión de obligaciones, cobros y pagos", "/erp/finanzas-contabilidad/cuentas-cobrar-pagar", 2);
        AddSub(erpFinanzas, "flujo-caja", "Flujo de caja", "Ingresos, egresos y proyección", "/erp/finanzas-contabilidad/flujo-caja", 3);
        AddSub(erpFinanzas, "utilidades-perdidas", "Utilidades y pérdidas", "Reporte de utilidades y pérdidas", "/erp/finanzas-contabilidad/utilidades-perdidas", 4);

        var erpCompras = AddModule(erp, "compras-proveedores", "Compras y Proveedores", "Órdenes, proveedores, costos y recepción", "shopping-cart", "/erp/compras-proveedores", 2);
        AddSub(erpCompras, "ordenes-compra", "Órdenes de compra", "Gestión de órdenes de compra", "/erp/compras-proveedores/ordenes-compra", 1);
        AddSub(erpCompras, "proveedores", "Proveedores", "Evaluación y gestión de proveedores", "/erp/compras-proveedores/proveedores", 2);
        AddSub(erpCompras, "costeo-adquisicion", "Costeo de adquisición", "Costeo de importación y adquisición", "/erp/compras-proveedores/costeo-adquisicion", 3);
        AddSub(erpCompras, "recepcion-mercaderia", "Recepción de mercadería", "Recepción vinculada al inventario", "/erp/compras-proveedores/recepcion-mercaderia", 4);

        var erpFacturacion = AddModule(erp, "facturacion", "Facturación Electrónica", "Comprobantes, SUNAT y notas", "receipt", "/erp/facturacion", 3);
        AddSub(erpFacturacion, "comprobantes", "Comprobantes", "Boletas, facturas y comprobantes electrónicos", "/erp/facturacion/comprobantes", 1);
        AddSub(erpFacturacion, "sunat-tiempo-real", "SUNAT en tiempo real", "Comunicación en tiempo real con SUNAT", "/erp/facturacion/sunat-tiempo-real", 2);
        AddSub(erpFacturacion, "notas", "Notas de crédito y débito", "Gestión de notas electrónicas", "/erp/facturacion/notas", 3);
        AddSub(erpFacturacion, "guias-remision", "Guías de remisión", "Emisión y seguimiento de guías", "/erp/facturacion/guias-remision", 4);

        var erpInventario = AddModule(erp, "inventario", "Inventario Central", "Productos, kardex, valoración, almacenes y alertas", "warehouse", "/erp/inventario", 4);
        AddSub(erpInventario, "productos", "Productos", "Catálogo de productos y categorías", "/erp/inventario/productos", 1);
        AddSub(erpInventario, "kardex", "Kardex", "Control de kardex general", "/erp/inventario/kardex", 2);
        AddSub(erpInventario, "valoracion", "Valoración de existencias", "Costo promedio y valoración de stock", "/erp/inventario/valoracion", 3);
        AddSub(erpInventario, "stock-minimo", "Alertas de stock mínimo", "Alertas y reposición por mínimos", "/erp/inventario/stock-minimo", 4);
        AddSub(erpInventario, "almacenes", "Almacenes", "Almacenes, ubicaciones y transferencias", "/erp/inventario/almacenes", 5);

        var pos = AddSystem("pos", "POS", "Venta en punto de venta y operaciones de tienda", "store", "/pos", 2);
        var posCaja = AddModule(pos, "caja-atencion", "Caja y Atención", "Caja, escaneo, arqueo y pagos", "scan-barcode", "/pos/caja-atencion", 1);
        AddSub(posCaja, "escaneo-codigo-barras", "Escaneo de código de barras", "Lectura de productos por código de barras", "/pos/caja-atencion/escaneo-codigo-barras", 1);
        AddSub(posCaja, "apertura-cierre-caja", "Apertura, cierre y arqueo", "Operaciones de caja y arqueos", "/pos/caja-atencion/apertura-cierre-caja", 2);
        AddSub(posCaja, "pasarelas-pago", "Pasarelas de pago", "Tarjetas, Yape, Plin y otros medios", "/pos/caja-atencion/pasarelas-pago", 3);
        var posInventario = AddModule(pos, "inventario-local", "Inventario Local", "Stock de tienda, mermas y recepción", "package-check", "/pos/inventario-local", 2);
        AddSub(posInventario, "stock-piso-venta", "Stock en piso de venta", "Stock físico en tienda", "/pos/inventario-local/stock-piso-venta", 1);
        AddSub(posInventario, "mermas-rapidas", "Mermas rápidas", "Registro rápido de mermas", "/pos/inventario-local/mermas-rapidas", 2);
        AddSub(posInventario, "recepcion-mercaderia", "Recepción de mercadería", "Recepción desde almacén central", "/pos/inventario-local/recepcion-mercaderia", 3);

        var crm = AddSystem("crm", "CRM", "Relación comercial, clientes, marketing y oportunidades", "users", "/crm", 3);
        var crmVentas = AddModule(crm, "ventas-b2b", "Ventas B2B y Distribución", "Pipeline, cotizaciones y seguimientos", "handshake", "/crm/ventas-b2b", 1);
        AddSub(crmVentas, "pipeline", "Pipeline", "Embudo de ventas", "/crm/ventas-b2b/pipeline", 1);
        AddSub(crmVentas, "cotizaciones", "Cotizaciones", "Historial de cotizaciones", "/crm/ventas-b2b/cotizaciones", 2);
        AddSub(crmVentas, "seguimientos", "Seguimientos", "Recordatorios de seguimiento de asesores", "/crm/ventas-b2b/seguimientos", 3);
        var crmMarketing = AddModule(crm, "marketing-automatico", "Marketing Automático", "Segmentación, campañas y fidelización", "megaphone", "/crm/marketing-automatico", 2);
        AddSub(crmMarketing, "segmentacion", "Segmentación", "Segmentación de clientes", "/crm/marketing-automatico/segmentacion", 1);
        AddSub(crmMarketing, "envios-masivos", "Envíos masivos", "Correos, WhatsApp y campañas programadas", "/crm/marketing-automatico/envios-masivos", 2);
        AddSub(crmMarketing, "cupones-fidelizacion", "Cupones de fidelización", "Gestión de cupones y fidelización", "/crm/marketing-automatico/cupones-fidelizacion", 3);

        var rh = AddSystem("rh", "Recursos Humanos", "Gestión integral de personal, nómina y asistencia", "id-card", "/rh", 4);
        var rhNomina = AddModule(rh, "nomina", "Nómina", "Sueldos, comisiones y descuentos de ley", "wallet-cards", "/rh/nomina", 1);
        AddSub(rhNomina, "sueldos-horas-extra", "Sueldos y horas extra", "Cálculo de sueldo básico y horas extra", "/rh/nomina/sueldos-horas-extra", 1);
        AddSub(rhNomina, "comisiones-ventas", "Comisiones por ventas", "Comisiones conectadas al POS", "/rh/nomina/comisiones-ventas", 2);
        AddSub(rhNomina, "descuentos-ley", "Descuentos de ley", "AFP, ONP, gratificaciones y CTS", "/rh/nomina/descuentos-ley", 3);
        var rhAsistencia = AddModule(rh, "asistencia-campo", "Control de Asistencia en Campo", "Marcaciones, turnos, faltas y tardanzas", "clock", "/rh/asistencia-campo", 2);
        AddSub(rhAsistencia, "marcacion-biometrica", "Marcación biométrica", "Huella, rostro y control biométrico", "/rh/asistencia-campo/marcacion-biometrica", 1);
        AddSub(rhAsistencia, "turnos-rotativos", "Turnos rotativos", "Gestión de turnos para tiendas", "/rh/asistencia-campo/turnos-rotativos", 2);
        AddSub(rhAsistencia, "faltas-tardanzas", "Faltas y tardanzas", "Registro de faltas y tardanzas", "/rh/asistencia-campo/faltas-tardanzas", 3);

        var oms = AddSystem("oms", "OMS", "Gestión centralizada de pedidos omnicanal", "package-search", "/oms", 5);
        var omsRutas = AddModule(oms, "enrutamiento-inteligente", "Enrutamiento Inteligente", "Asignación de stock y flujos click and collect", "route", "/oms/enrutamiento-inteligente", 1);
        AddSub(omsRutas, "asignacion-stock", "Asignación de stock", "Asignación por cercanía y disponibilidad", "/oms/enrutamiento-inteligente/asignacion-stock", 1);
        AddSub(omsRutas, "click-and-collect", "Click and Collect", "Compra web y retiro en tienda", "/oms/enrutamiento-inteligente/click-and-collect", 2);
        var omsCanales = AddModule(oms, "consolidacion-canales", "Consolidación de Canales", "E-commerce, WhatsApp y marketplaces", "network", "/oms/consolidacion-canales", 2);
        AddSub(omsCanales, "ecommerce", "E-commerce", "Integración con tienda online", "/oms/consolidacion-canales/ecommerce", 1);
        AddSub(omsCanales, "whatsapp-business", "WhatsApp Business", "API de WhatsApp Business", "/oms/consolidacion-canales/whatsapp-business", 2);
        AddSub(omsCanales, "marketplaces", "Marketplaces", "Sincronización con marketplaces", "/oms/consolidacion-canales/marketplaces", 3);

        var wms = AddSystem("wms", "WMS", "Control de operaciones de almacén", "warehouse", "/wms", 6);
        var wmsOperaciones = AddModule(wms, "operaciones-almacen", "Operaciones de Almacén", "Ubicaciones, picking y packing", "boxes", "/wms/operaciones-almacen", 1);
        AddSub(wmsOperaciones, "ubicaciones", "Ubicaciones", "Mapeo de pasillo, estante y altura", "/wms/operaciones-almacen/ubicaciones", 1);
        AddSub(wmsOperaciones, "picking-packing", "Picking y packing", "Rutas para operarios", "/wms/operaciones-almacen/picking-packing", 2);
        var wmsDistribucion = AddModule(wms, "distribucion-interna", "Distribución Interna", "Transferencias y despacho interno", "truck", "/wms/distribucion-interna", 2);
        AddSub(wmsDistribucion, "transferencias-almacenes", "Transferencias entre almacenes", "Gestión de transferencias internas", "/wms/distribucion-interna/transferencias-almacenes", 1);
        AddSub(wmsDistribucion, "despacho-reabastecimiento", "Despacho de reabastecimiento", "Despacho de camiones para sucursales", "/wms/distribucion-interna/despacho-reabastecimiento", 2);

        var tms = AddSystem("tms", "TMS", "Transporte, última milla y despachos", "truck", "/tms", 7);
        var tmsPlanificacion = AddModule(tms, "planificacion-rutas", "Planificación de Rutas", "Optimización y asignación por capacidad", "map", "/tms/planificacion-rutas", 1);
        AddSub(tmsPlanificacion, "optimizacion-recorridos", "Optimización de recorridos", "Rutas para transportistas", "/tms/planificacion-rutas/optimizacion-recorridos", 1);
        AddSub(tmsPlanificacion, "asignacion-carga", "Asignación de carga", "Asignación por capacidad del vehículo", "/tms/planificacion-rutas/asignacion-carga", 2);
        var tmsSeguimiento = AddModule(tms, "seguimiento-entrega", "Seguimiento y Entrega", "GPS y prueba de entrega", "map-pinned", "/tms/seguimiento-entrega", 2);
        AddSub(tmsSeguimiento, "rastreo-gps", "Rastreo GPS", "Seguimiento en tiempo real", "/tms/seguimiento-entrega/rastreo-gps", 1);
        AddSub(tmsSeguimiento, "prueba-entrega", "Prueba de entrega", "Firma digital y confirmación de entrega", "/tms/seguimiento-entrega/prueba-entrega", 2);

        var pim = AddSystem("pim", "PIM", "Catálogo único e información enriquecida de productos", "clipboard-list", "/pim", 8);
        var pimContenido = AddModule(pim, "contenido-producto", "Contenido de Producto", "Ficha técnica, atributos y multimedia", "image", "/pim/contenido-producto", 1);
        AddSub(pimContenido, "ficha-tecnica", "Ficha técnica", "Talla, color, marca y características", "/pim/contenido-producto/ficha-tecnica", 1);
        AddSub(pimContenido, "multimedia", "Multimedia", "Repositorio de fotos y videos de producto", "/pim/contenido-producto/multimedia", 2);
        var pimSync = AddModule(pim, "sincronizacion", "Sincronización", "Actualización masiva hacia canales", "refresh-cw", "/pim/sincronizacion", 2);
        AddSub(pimSync, "actualizacion-masiva", "Actualización masiva", "Precios y datos hacia POS, web y apps", "/pim/sincronizacion/actualizacion-masiva", 1);

        var sfa = AddSystem("sfa", "SFA", "Automatización de ventas en campo", "smartphone", "/sfa", 9);
        var sfaPreventa = AddModule(sfa, "preventa-movil", "Preventa Móvil", "Pedidos, stock y cobranzas en ruta", "tablet-smartphone", "/sfa/preventa-movil", 1);
        AddSub(sfaPreventa, "pedidos-ruta", "Pedidos en ruta", "Toma de pedidos offline y online", "/sfa/preventa-movil/pedidos-ruta", 1);
        AddSub(sfaPreventa, "consulta-stock", "Consulta de stock", "Stock central en tiempo real", "/sfa/preventa-movil/consulta-stock", 2);
        AddSub(sfaPreventa, "cobranzas", "Cobranzas", "Registro de cobranzas de clientes morosos", "/sfa/preventa-movil/cobranzas", 3);

        var helpDesk = AddSystem("help-desk", "Help Desk", "Soporte, tickets y postventa", "headphones", "/help-desk", 10);
        var tickets = AddModule(helpDesk, "tickets", "Tickets", "Reclamos, asignación y SLA", "ticket", "/help-desk/tickets", 1);
        AddSub(tickets, "reclamos-omnicanal", "Reclamos omnicanal", "Centralización de reclamos", "/help-desk/tickets/reclamos-omnicanal", 1);
        AddSub(tickets, "asignacion-automatica", "Asignación automática", "Asignación automática a agentes", "/help-desk/tickets/asignacion-automatica", 2);
        AddSub(tickets, "sla", "SLA", "Medición de tiempos de respuesta", "/help-desk/tickets/sla", 3);

        var analytics = AddSystem("retail-analytics", "Retail Analytics", "Tráfico e inteligencia de tienda", "chart-line", "/retail-analytics", 11);
        var trafico = AddModule(analytics, "conteo-personas", "Conteo de Personas", "Sensores, conversión y mapas de calor", "footprints", "/retail-analytics/conteo-personas", 1);
        AddSub(trafico, "sensores-camaras", "Sensores y cámaras IA", "Conteo de ingreso con sensores y cámaras", "/retail-analytics/conteo-personas/sensores-camaras", 1);
        AddSub(trafico, "tasa-conversion", "Tasa de conversión", "Visitas contra tickets POS", "/retail-analytics/conteo-personas/tasa-conversion", 2);
        AddSub(trafico, "mapas-calor", "Mapas de calor", "Mapas de calor en pasillos", "/retail-analytics/conteo-personas/mapas-calor", 3);

        var prevencion = AddSystem("prevencion-perdidas", "Prevención de Pérdidas", "Auditoría y control preventivo de stock", "shield-alert", "/prevencion-perdidas", 12);
        var inventarios = AddModule(prevencion, "inventarios-ciclicos", "Inventarios Cíclicos", "Auditoría aleatoria y alertas de descuadres", "search-check", "/prevencion-perdidas/inventarios-ciclicos", 1);
        AddSub(inventarios, "auditoria-aleatoria", "Auditoría aleatoria", "Auditoría diaria por categorías", "/prevencion-perdidas/inventarios-ciclicos/auditoria-aleatoria", 1);
        AddSub(inventarios, "alertas-descuadres", "Alertas de descuadres", "Anulaciones frecuentes y descuadres sospechosos", "/prevencion-perdidas/inventarios-ciclicos/alertas-descuadres", 2);

        var bi = AddSystem("bi", "BI", "Inteligencia de negocio y análisis consolidado", "chart-pie", "/bi", 13);
        var dashboards = AddModule(bi, "dashboards", "Dashboards", "Rentabilidad, ventas y rotación", "layout-panel-top", "/bi/dashboards", 1);
        AddSub(dashboards, "rentabilidad", "Rentabilidad", "Panel de rentabilidad neta y bruta", "/bi/dashboards/rentabilidad", 1);
        AddSub(dashboards, "comparativa-ventas", "Comparativa de ventas", "Comparativa de ventas entre tiendas", "/bi/dashboards/comparativa-ventas", 2);
        AddSub(dashboards, "productos-baja-rotacion", "Productos de baja rotación", "Análisis de productos estancados", "/bi/dashboards/productos-baja-rotacion", 3);

        return systems;
    }

    private static string MapKey(string systemKey, string moduleKey) => $"{systemKey}.{moduleKey}";

    private sealed record SystemSeed(string Key, string Name, string Description, string Icon, string BasePath, int DisplayOrder, List<ModuleSeed> Modules);
    private sealed record ModuleSeed(string Key, string Name, string Description, string Icon, string BasePath, int DisplayOrder, List<SubModuleSeed> SubModules);
    private sealed record SubModuleSeed(string Key, string Name, string Description, string RoutePath, int DisplayOrder);
    private sealed record SubModuleSeedResult(string SystemKey, string ModuleKey, SaasSubModule SubModule);
}
