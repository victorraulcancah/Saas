using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend_Saas.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;

        var roleManager = sp.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
        var db = sp.GetRequiredService<AppDbContext>();

        if (await db.Modules.AnyAsync()) return;

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

        var modFacturacion = new Module
        {
            Id = Guid.NewGuid(),
            Name = "Facturación SAT",
            Key = "facturacion",
            Description = "Módulo de facturación electrónica CFDI 4.0",
            Icon = "receipt",
            BasePath = "/facturacion",
            DisplayOrder = 1,
            IsActive = true
        };

        var modInventario = new Module
        {
            Id = Guid.NewGuid(),
            Name = "Inventarios",
            Key = "inventarios",
            Description = "Módulo de control de inventarios",
            Icon = "inventory",
            BasePath = "/inventarios",
            DisplayOrder = 2,
            IsActive = true
        };

        var modClientes = new Module
        {
            Id = Guid.NewGuid(),
            Name = "Clientes",
            Key = "clientes",
            Description = "Módulo de gestión de clientes",
            Icon = "people",
            BasePath = "/clientes",
            DisplayOrder = 3,
            IsActive = true
        };

        var modRH = new Module
        {
            Id = Guid.NewGuid(),
            Name = "Recursos Humanos",
            Key = "rh",
            Description = "Módulo de recursos humanos",
            Icon = "badge",
            BasePath = "/rh",
            DisplayOrder = 4,
            IsActive = true
        };

        db.Modules.AddRange(modFacturacion, modInventario, modClientes, modRH);

        var permisosFacturacion = new[]
        {
            new Permission { Id = Guid.NewGuid(), ModuleId = modFacturacion.Id, Name = "Generar CFDIs", Key = "facturacion.generar", Description = "Generar facturas electrónicas" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modFacturacion.Id, Name = "Cancelar CFDIs", Key = "facturacion.cancelar", Description = "Cancelar facturas electrónicas" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modFacturacion.Id, Name = "Configurar CSD", Key = "facturacion.configurar-csd", Description = "Gestionar certificados CSD (.cer/.key)" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modFacturacion.Id, Name = "Configurar PAC", Key = "facturacion.configurar-pac", Description = "Configurar proveedor PAC" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modFacturacion.Id, Name = "Ver CFDIs", Key = "facturacion.ver", Description = "Visualizar facturas emitidas" },
        };

        var permisosInventario = new[]
        {
            new Permission { Id = Guid.NewGuid(), ModuleId = modInventario.Id, Name = "Ver inventario", Key = "inventarios.ver" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modInventario.Id, Name = "Agregar producto", Key = "inventarios.agregar" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modInventario.Id, Name = "Ajustar stock", Key = "inventarios.ajustar" },
        };

        var permisosClientes = new[]
        {
            new Permission { Id = Guid.NewGuid(), ModuleId = modClientes.Id, Name = "Ver clientes", Key = "clientes.ver" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modClientes.Id, Name = "Crear cliente", Key = "clientes.crear" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modClientes.Id, Name = "Editar cliente", Key = "clientes.editar" },
        };

        var permisosRH = new[]
        {
            new Permission { Id = Guid.NewGuid(), ModuleId = modRH.Id, Name = "Ver empleados", Key = "rh.ver" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modRH.Id, Name = "Registrar empleado", Key = "rh.registrar" },
            new Permission { Id = Guid.NewGuid(), ModuleId = modRH.Id, Name = "Gestionar nómina", Key = "rh.nomina" },
        };

        db.Permissions.AddRange(permisosFacturacion);
        db.Permissions.AddRange(permisosInventario);
        db.Permissions.AddRange(permisosClientes);
        db.Permissions.AddRange(permisosRH);

        await db.SaveChangesAsync();
    }
}
