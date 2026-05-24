namespace Backend.Application.Common.Interfaces;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(string email, string password, string? firstName, string? lastName, Guid? tenantId);
}

public record AuthResult(string Token, string Email, string Role, Guid? TenantId);
