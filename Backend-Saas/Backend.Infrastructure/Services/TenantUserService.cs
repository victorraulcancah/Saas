using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class TenantUserService : ITenantUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;
    private readonly IAuditService _audit;

    public TenantUserService(UserManager<ApplicationUser> userManager, AppDbContext context, IAuditService audit)
    {
        _userManager = userManager;
        _context = context;
        _audit = audit;
    }

    public async Task<ApplicationUser> CreateTenantUserAsync(Guid tenantId, string email, string password, string? firstName, string? lastName)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName ?? string.Empty,
            LastName = lastName ?? string.Empty,
            TenantId = tenantId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "AdminTenant");
        await _audit.LogAsync("CREATE_USER", "ApplicationUser", user.Id.ToString(), $"Usuario {email} creado para tenant {tenantId}");
        return user;
    }

    public async Task<IEnumerable<ApplicationUser>> GetTenantUsersAsync(Guid tenantId)
    {
        return await _context.Users
            .Where(u => u.TenantId == tenantId && !u.IsDeleted)
            .ToListAsync();
    }
}
