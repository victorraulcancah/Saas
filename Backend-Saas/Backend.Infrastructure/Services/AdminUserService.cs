using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class AdminUserService : IAdminUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AdminUserService(UserManager<ApplicationUser> userManager, AppDbContext context, ICurrentUserService currentUser)
    {
        _userManager = userManager;
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
    {
        return await _context.Users
            .Where(u => u.TenantId == _currentUser.TenantId && !u.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ApplicationUser> CreateUserAsync(string email, string password, string? firstName, string? lastName, List<Guid>? roleIds)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName ?? string.Empty,
            LastName = lastName ?? string.Empty,
            TenantId = _currentUser.TenantId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        if (roleIds?.Any() == true)
        {
            var roles = await _context.Roles
                .Where(r => roleIds.Contains(r.Id) && r.TenantId == _currentUser.TenantId)
                .Select(r => r.Name!)
                .ToListAsync();

            if (roles.Any())
                await _userManager.AddToRolesAsync(user, roles);
        }

        return user;
    }
}
