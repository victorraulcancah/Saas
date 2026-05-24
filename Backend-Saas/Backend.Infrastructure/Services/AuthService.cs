using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwt;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwt)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwt = jwt;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null || user.IsDeleted)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Usuario";
        var token = _jwt.GenerateToken(user.Id, user.Email!, role, user.TenantId);

        return new AuthResult(token, user.Email!, role, user.TenantId);
    }

    public async Task<AuthResult> RegisterAsync(string email, string password, string? firstName, string? lastName, Guid? tenantId)
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

        await _userManager.AddToRoleAsync(user, "Usuario");

        var token = _jwt.GenerateToken(user.Id, user.Email!, "Usuario", user.TenantId);
        return new AuthResult(token, user.Email!, "Usuario", user.TenantId);
    }
}
