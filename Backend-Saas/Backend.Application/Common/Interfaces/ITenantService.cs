namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Common;

public interface ITenantService
{
    Task<Tenant> CreateTenantAsync(string name, string slug, string? email, string? subscriptionPlan);
    Task<Tenant?> GetTenantByIdAsync(Guid id);
    Task<Tenant> UpdateTenantAsync(Guid id, string name, string slug, string? email, string? subscriptionPlan, bool isActive);
    Task DeleteTenantAsync(Guid id);
    Task<IEnumerable<Tenant>> GetAllTenantsAsync();
    Task<Tenant?> GetTenantBySlugAsync(string slug);
}
