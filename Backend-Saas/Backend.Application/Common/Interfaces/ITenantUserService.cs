namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Common;

public interface ITenantUserService
{
    Task<ApplicationUser> CreateTenantUserAsync(Guid tenantId, string email, string password, string? firstName, string? lastName);
    Task<IEnumerable<ApplicationUser>> GetTenantUsersAsync(Guid tenantId);
}
