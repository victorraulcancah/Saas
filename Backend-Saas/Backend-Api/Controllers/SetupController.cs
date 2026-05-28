using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _db;

        public SetupController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, AppDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            if (await _db.SaasSystems.AnyAsync())
                return Ok(new { message = "Ya hay datos semilla. No se ejecutó el seed." });

            var superAdminRole = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = "SuperAdmin",
                Description = "Super administrador global del sistema",
                CreatedAt = DateTime.UtcNow
            };
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
                await _roleManager.CreateAsync(superAdminRole);

            if (!await _roleManager.RoleExistsAsync("AdminTenant"))
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = "AdminTenant",
                    Description = "Administrador de un tenant",
                    CreatedAt = DateTime.UtcNow
                });

            if (!await _roleManager.RoleExistsAsync("Usuario"))
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = "Usuario",
                    Description = "Usuario regular del sistema",
                    CreatedAt = DateTime.UtcNow
                });

            var superAdmin = await _userManager.FindByEmailAsync("admin@saas.com");
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
                var result = await _userManager.CreateAsync(superAdmin, "Admin123!");
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
            }

            await SeedData.Initialize(HttpContext.RequestServices);

            return Ok(new { message = "Seed ejecutado correctamente. SuperAdmin: admin@saas.com / Admin123!" });
        }

        [HttpGet("status")]
        public async Task<IActionResult> Status()
        {
            var admin = await _userManager.FindByEmailAsync("admin@saas.com");
            var roles = admin is not null ? await _userManager.GetRolesAsync(admin) : [];
            return Ok(new
            {
                superAdminExists = admin is not null,
                roles = roles,
                systemsCount = await _db.SaasSystems.CountAsync(),
                modulesCount = await _db.SaasModules.CountAsync(),
                subModulesCount = await _db.SaasSubModules.CountAsync(),
                tenantsCount = await _db.Tenants.CountAsync()
            });
        }
    }
}