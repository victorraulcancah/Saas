namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Common;

public interface ITenantService
{
    Task<Tenant> CreateTenantAsync(string name, string slug, string? email, string? subscriptionPlan);
    Task AssignModuleAsync(Guid tenantId, Guid moduleId, string? config);
    Task<IEnumerable<Tenant>> GetAllTenantsAsync();
    Task<Tenant?> GetTenantBySlugAsync(string slug);
}
