using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Backend_Saas.DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Saas.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "AdminTenant")]
public class RolesController : ControllerBase
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly AppDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public RolesController(RoleManager<ApplicationRole> roleManager, AppDbContext db, ICurrentUserService currentUser)
    {
        _roleManager = roleManager;
        _db = db;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _db.Roles
            .Where(r => r.TenantId == _currentUser.TenantId && !r.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        var response = roles.Select(r => new RoleResponse(r.Id, r.Name!, r.Description, r.CreatedAt));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
    {
        var role = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            TenantId = _currentUser.TenantId
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (request.PermissionIds?.Any() == true)
        {
            foreach (var permissionId in request.PermissionIds)
            {
                _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permissionId });
            }
            await _db.SaveChangesAsync();
        }

        return Ok(new RoleResponse(role.Id, role.Name!, role.Description, role.CreatedAt));
    }
}
