using Backend.Application.Common.Interfaces;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class UserPermissionService : IUserPermissionService
{
    private readonly AppDbContext _context;

    public UserPermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string permissionKey)
    {
        if (string.IsNullOrWhiteSpace(permissionKey))
            return false;

        var roleIds = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        if (roleIds.Count == 0)
            return false;

        var hasPrivilegedRole = await _context.Roles
            .IgnoreQueryFilters()
            .AnyAsync(r => roleIds.Contains(r.Id) && (r.Name == "SuperAdmin" || r.Name == "AdminTenant"));

        if (hasPrivilegedRole)
            return true;

        var normalizedKey = permissionKey.Trim().ToLowerInvariant();

        return await _context.RolePermissions
            .AsNoTracking()
            .AnyAsync(rp =>
                roleIds.Contains(rp.RoleId) &&
                !rp.IsDeleted &&
                rp.Permission.IsActive &&
                rp.Permission.Key.ToLower() == normalizedKey);
    }
}
