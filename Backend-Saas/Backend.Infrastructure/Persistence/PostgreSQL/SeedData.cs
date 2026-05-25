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

        var superAdminRole = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = "SuperAdmin",
            Description = "Super administrador global del sistema",
            CreatedAt = DateTime.UtcNow
        };

        if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            await roleManager.CreateAsync(superAdminRole);

        if (!await roleManager.RoleExistsAsync("AdminTenant"))
            await roleManager.CreateAsync(new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = "AdminTenant",
                Description = "Administrador de un tenant",
                CreatedAt = DateTime.UtcNow
            });

        if (!await roleManager.RoleExistsAsync("Usuario"))
            await roleManager.CreateAsync(new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = "Usuario",
                Description = "Usuario regular del sistema",
                CreatedAt = DateTime.UtcNow
            });

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

        if (!await db.SaasSystems.AnyAsync())
        {
            await SeedSaasCentralCatalog(db);
        }
    }

    private static async Task SeedSaasCentralCatalog(AppDbContext db)
    {
        var saas = new SaasSystem
        {
            Id = Guid.NewGuid(),
            Name = "Plataforma SaaS Central",
            Key = "saas-central",
            Description = "Venta, control y gobierno de sistemas, módulos, submódulos, tenants, licencias y suscripciones",
            Icon = "layout-dashboard",
            BasePath = "/superadmin",
            DisplayOrder = 0
        };

        var catalog = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = saas.Id,
            Name = "Catálogo Comercial",
            Key = "catalogo-comercial",
            Description = "Define sistemas, módulos y submódulos vendibles",
            Icon = "boxes",
            BasePath = "/superadmin/saas-catalog",
            DisplayOrder = 1
        };

        var tenants = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = saas.Id,
            Name = "Clientes y Tenants",
            Key = "clientes-tenants",
            Description = "Administra empresas clientes, perfiles fiscales y sucursales",
            Icon = "building-2",
            BasePath = "/superadmin/tenants",
            DisplayOrder = 2
        };

        var billing = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = saas.Id,
            Name = "Suscripciones y Licencias",
            Key = "suscripciones-licencias",
            Description = "Controla planes, pagos, sistemas activos, módulos y submódulos por empresa",
            Icon = "badge-check",
            BasePath = "/superadmin/subscriptions",
            DisplayOrder = 3
        };

        var sunat = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = saas.Id,
            Name = "Configuración Fiscal SUNAT",
            Key = "configuracion-sunat",
            Description = "Gestiona usuario SOL, certificado digital, OSE/PSE, series y correlativos",
            Icon = "file-check-2",
            BasePath = "/superadmin/sunat",
            DisplayOrder = 4
        };

        var subModules = new[]
        {
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = catalog.Id, Name = "Sistemas vendibles", Key = "sistemas", Description = "ERP, POS, CRM, RH y demás sistemas", RoutePath = "/superadmin/saas-catalog/systems", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = catalog.Id, Name = "Módulos vendibles", Key = "modulos", Description = "Procesos agrupados dentro de cada sistema", RoutePath = "/superadmin/saas-catalog/modules", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = catalog.Id, Name = "Submódulos vendibles", Key = "submodulos", Description = "Funciones específicas activables por plan o add-on", RoutePath = "/superadmin/saas-catalog/submodules", DisplayOrder = 3 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = tenants.Id, Name = "Perfil legal de empresa", Key = "perfil-legal", Description = "RUC, razón social, dirección fiscal, ubigeo y datos SUNAT públicos", RoutePath = "/superadmin/tenants/company-profile", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = tenants.Id, Name = "Sucursales", Key = "sucursales", Description = "Locales, establecimientos SUNAT y direcciones operativas", RoutePath = "/superadmin/tenants/branches", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = billing.Id, Name = "Planes", Key = "planes", Description = "Starter, Professional, Enterprise y planes personalizados", RoutePath = "/superadmin/subscriptions/plans", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = billing.Id, Name = "Licencias por empresa", Key = "licencias", Description = "Sistemas, módulos y submódulos habilitados por tenant", RoutePath = "/superadmin/subscriptions/licenses", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = sunat.Id, Name = "Credenciales SOL", Key = "credenciales-sol", Description = "Usuario SOL y clave SOL cifrada", RoutePath = "/superadmin/sunat/credentials", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = sunat.Id, Name = "Certificado digital", Key = "certificado-digital", Description = "Archivo PEM/PFX y clave cifrada", RoutePath = "/superadmin/sunat/certificate", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = sunat.Id, Name = "Series y correlativos", Key = "series-correlativos", Description = "Series por tipo de comprobante y sucursal", RoutePath = "/superadmin/sunat/series", DisplayOrder = 3 },
        };

        var erp = new SaasSystem
        {
            Id = Guid.NewGuid(),
            Name = "ERP",
            Key = "erp",
            Description = "Núcleo financiero, contable, compras, inventario y facturación",
            Icon = "landmark",
            BasePath = "/erp",
            DisplayOrder = 1
        };

        var crm = new SaasSystem
        {
            Id = Guid.NewGuid(),
            Name = "CRM",
            Key = "crm",
            Description = "Gestión comercial, clientes, oportunidades y atención",
            Icon = "users",
            BasePath = "/crm",
            DisplayOrder = 3
        };

        var rh = new SaasSystem
        {
            Id = Guid.NewGuid(),
            Name = "Recursos Humanos",
            Key = "rh",
            Description = "Gestión de personal, asistencia y planilla",
            Icon = "id-card",
            BasePath = "/rh",
            DisplayOrder = 4
        };

        var facturacion = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = erp.Id,
            Name = "Facturación Electrónica SUNAT",
            Key = "facturacion",
            Description = "Emisión, consulta y anulación de comprobantes electrónicos",
            Icon = "receipt",
            BasePath = "/erp/facturacion",
            DisplayOrder = 1
        };

        var inventario = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = erp.Id,
            Name = "Inventario Central",
            Key = "inventario",
            Description = "Catálogo, stock, almacenes, movimientos y kardex",
            Icon = "warehouse",
            BasePath = "/erp/inventario",
            DisplayOrder = 2
        };

        var compras = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = erp.Id,
            Name = "Compras y Proveedores",
            Key = "compras-proveedores",
            Description = "Gestión de proveedores, órdenes de compra, recepción y cuentas por pagar",
            Icon = "shopping-cart",
            BasePath = "/erp/compras-proveedores",
            DisplayOrder = 3
        };

        var finanzas = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = erp.Id,
            Name = "Finanzas y Contabilidad",
            Key = "finanzas-contabilidad",
            Description = "Contabilidad, cuentas por cobrar/pagar, flujo de caja y reportes financieros",
            Icon = "chart-no-axes-combined",
            BasePath = "/erp/finanzas-contabilidad",
            DisplayOrder = 4
        };

        var clientes = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = crm.Id,
            Name = "Clientes",
            Key = "clientes",
            Description = "Gestión de clientes, contactos y actividad comercial",
            Icon = "contact",
            BasePath = "/crm/clientes",
            DisplayOrder = 1
        };

        var recursosHumanos = new SaasModule
        {
            Id = Guid.NewGuid(),
            SystemId = rh.Id,
            Name = "Gestión de Personal",
            Key = "personal",
            Description = "Empleados, asistencia, nómina y documentos laborales",
            Icon = "briefcase-business",
            BasePath = "/rh/personal",
            DisplayOrder = 1
        };

        var businessSubModules = new[]
        {
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = facturacion.Id, Name = "Facturas y boletas", Key = "comprobantes", Description = "Emisión de facturas, boletas y consulta de estado SUNAT", RoutePath = "/erp/facturacion/comprobantes", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = facturacion.Id, Name = "Notas de crédito y débito", Key = "notas", Description = "Gestión de notas de crédito y débito electrónicas", RoutePath = "/erp/facturacion/notas", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = facturacion.Id, Name = "Guías de remisión", Key = "guias-remision", Description = "Emisión y seguimiento de guías de remisión electrónica", RoutePath = "/erp/facturacion/guias-remision", DisplayOrder = 3 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = inventario.Id, Name = "Productos", Key = "productos", Description = "Catálogo de productos y categorías", RoutePath = "/erp/inventario/productos", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = inventario.Id, Name = "Kardex", Key = "kardex", Description = "Movimientos, valorización y trazabilidad de stock", RoutePath = "/erp/inventario/kardex", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = inventario.Id, Name = "Almacenes", Key = "almacenes", Description = "Almacenes, ubicaciones y transferencias", RoutePath = "/erp/inventario/almacenes", DisplayOrder = 3 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = compras.Id, Name = "Proveedores", Key = "proveedores", Description = "Registro, evaluación y gestión de proveedores", RoutePath = "/erp/compras-proveedores/proveedores", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = compras.Id, Name = "Órdenes de compra", Key = "ordenes-compra", Description = "Solicitudes, aprobación y seguimiento de órdenes de compra", RoutePath = "/erp/compras-proveedores/ordenes-compra", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = compras.Id, Name = "Recepción de mercadería", Key = "recepcion-mercaderia", Description = "Conformidad, recepción y vínculo con inventario", RoutePath = "/erp/compras-proveedores/recepcion-mercaderia", DisplayOrder = 3 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = finanzas.Id, Name = "Contabilidad general", Key = "contabilidad-general", Description = "Libro diario, libro mayor y plan contable", RoutePath = "/erp/finanzas-contabilidad/contabilidad-general", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = finanzas.Id, Name = "Cuentas por cobrar y pagar", Key = "cuentas-cobrar-pagar", Description = "Obligaciones, vencimientos, cobros y pagos", RoutePath = "/erp/finanzas-contabilidad/cuentas-cobrar-pagar", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = finanzas.Id, Name = "Flujo de caja", Key = "flujo-caja", Description = "Ingresos, egresos y proyección de caja", RoutePath = "/erp/finanzas-contabilidad/flujo-caja", DisplayOrder = 3 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = clientes.Id, Name = "Cartera de clientes", Key = "cartera", Description = "Registro, edición y consulta de clientes", RoutePath = "/crm/clientes/cartera", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = clientes.Id, Name = "Actividades comerciales", Key = "actividades", Description = "Seguimientos, llamadas, reuniones y recordatorios", RoutePath = "/crm/clientes/actividades", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = recursosHumanos.Id, Name = "Empleados", Key = "empleados", Description = "Legajo digital y datos laborales", RoutePath = "/rh/personal/empleados", DisplayOrder = 1 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = recursosHumanos.Id, Name = "Asistencia", Key = "asistencia", Description = "Marcaciones, tardanzas, faltas y turnos", RoutePath = "/rh/personal/asistencia", DisplayOrder = 2 },
            new SaasSubModule { Id = Guid.NewGuid(), ModuleId = recursosHumanos.Id, Name = "Planilla", Key = "planilla", Description = "Cálculo de sueldos, descuentos y beneficios", RoutePath = "/rh/personal/planilla", DisplayOrder = 3 },
        };

        var starter = new SaasPlan
        {
            Id = Guid.NewGuid(),
            Name = "Starter",
            Key = "starter",
            Description = "Plan inicial para empresas pequeñas",
            Price = 99,
            Currency = "PEN",
            BillingCycle = "Monthly",
            MaxUsers = 5,
            MaxBranches = 1,
            MaxDocumentsPerMonth = 500
        };

        db.SaasSystems.AddRange(saas, erp, crm, rh);
        db.SaasModules.AddRange(catalog, tenants, billing, sunat, facturacion, inventario, compras, finanzas, clientes, recursosHumanos);
        db.SaasSubModules.AddRange(subModules);
        db.SaasSubModules.AddRange(businessSubModules);
        db.SaasPlans.Add(starter);
        db.PlanSystems.AddRange(
            new PlanSystem { PlanId = starter.Id, SystemId = saas.Id },
            new PlanSystem { PlanId = starter.Id, SystemId = erp.Id },
            new PlanSystem { PlanId = starter.Id, SystemId = crm.Id }
        );
        db.PlanModules.AddRange(
            new PlanModule { PlanId = starter.Id, ModuleId = catalog.Id },
            new PlanModule { PlanId = starter.Id, ModuleId = tenants.Id },
            new PlanModule { PlanId = starter.Id, ModuleId = billing.Id },
            new PlanModule { PlanId = starter.Id, ModuleId = sunat.Id },
            new PlanModule { PlanId = starter.Id, ModuleId = facturacion.Id },
            new PlanModule { PlanId = starter.Id, ModuleId = inventario.Id },
            new PlanModule { PlanId = starter.Id, ModuleId = compras.Id },
            new PlanModule { PlanId = starter.Id, ModuleId = clientes.Id }
        );
        db.PlanSubModules.AddRange(subModules.Select(sm => new PlanSubModule { PlanId = starter.Id, SubModuleId = sm.Id }));
        db.PlanSubModules.AddRange(businessSubModules
            .Where(sm => sm.ModuleId != recursosHumanos.Id)
            .Select(sm => new PlanSubModule { PlanId = starter.Id, SubModuleId = sm.Id }));

        db.Permissions.AddRange(subModules.SelectMany(sm => new[]
        {
            new Permission
            {
                Id = Guid.NewGuid(),
                SaasSystemId = saas.Id,
                SaasModuleId = sm.ModuleId,
                SaasSubModuleId = sm.Id,
                Name = $"Ver {sm.Name}",
                Key = $"saas-central.{sm.Key}.ver",
                Action = "view",
                Description = $"Permite visualizar {sm.Name}"
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                SaasSystemId = saas.Id,
                SaasModuleId = sm.ModuleId,
                SaasSubModuleId = sm.Id,
                Name = $"Administrar {sm.Name}",
                Key = $"saas-central.{sm.Key}.administrar",
                Action = "manage",
                Description = $"Permite administrar {sm.Name}"
            }
        }));

        db.Permissions.AddRange(businessSubModules.SelectMany(sm => new[]
        {
            new Permission
            {
                Id = Guid.NewGuid(),
                SaasSystemId = sm.ModuleId == clientes.Id ? crm.Id : sm.ModuleId == recursosHumanos.Id ? rh.Id : erp.Id,
                SaasModuleId = sm.ModuleId,
                SaasSubModuleId = sm.Id,
                Name = $"Ver {sm.Name}",
                Key = $"{GetSystemKey(sm.ModuleId, clientes.Id, recursosHumanos.Id)}.{GetModuleKey(sm.ModuleId, facturacion.Id, inventario.Id, compras.Id, finanzas.Id, clientes.Id, recursosHumanos.Id)}.{sm.Key}.ver",
                Action = "view",
                Description = $"Permite visualizar {sm.Name}"
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                SaasSystemId = sm.ModuleId == clientes.Id ? crm.Id : sm.ModuleId == recursosHumanos.Id ? rh.Id : erp.Id,
                SaasModuleId = sm.ModuleId,
                SaasSubModuleId = sm.Id,
                Name = $"Administrar {sm.Name}",
                Key = $"{GetSystemKey(sm.ModuleId, clientes.Id, recursosHumanos.Id)}.{GetModuleKey(sm.ModuleId, facturacion.Id, inventario.Id, compras.Id, finanzas.Id, clientes.Id, recursosHumanos.Id)}.{sm.Key}.administrar",
                Action = "manage",
                Description = $"Permite administrar {sm.Name}"
            }
        }));

        await db.SaveChangesAsync();
    }

    private static string GetSystemKey(Guid moduleId, Guid clientesId, Guid recursosHumanosId)
    {
        return moduleId == clientesId ? "crm" : moduleId == recursosHumanosId ? "rh" : "erp";
    }

    private static string GetModuleKey(Guid moduleId, Guid facturacionId, Guid inventarioId, Guid comprasId, Guid finanzasId, Guid clientesId, Guid recursosHumanosId)
    {
        if (moduleId == facturacionId) return "facturacion";
        if (moduleId == inventarioId) return "inventario";
        if (moduleId == comprasId) return "compras-proveedores";
        if (moduleId == finanzasId) return "finanzas-contabilidad";
        if (moduleId == clientesId) return "clientes";
        return "personal";
    }
}
