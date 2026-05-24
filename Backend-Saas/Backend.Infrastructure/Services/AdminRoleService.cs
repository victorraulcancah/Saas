using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class AdminRoleService : IAdminRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AdminRoleService(RoleManager<ApplicationRole> roleManager, AppDbContext context, ICurrentUserService currentUser)
    {
        _roleManager = roleManager;
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
    {
        return await _context.Roles
            .Where(r => r.TenantId == _currentUser.TenantId && !r.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ApplicationRole> CreateRoleAsync(string name, string? description, List<Guid>? permissionIds)
    {
        var role = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description ?? string.Empty,
            TenantId = _currentUser.TenantId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        if (permissionIds?.Any() == true)
        {
            foreach (var permissionId in permissionIds)
            {
                _context.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permissionId });
            }
            await _context.SaveChangesAsync();
        }

        return role;
    }
}
