namespace Backend.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsSuperAdmin { get; }
}
