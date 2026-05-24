namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Common;

public interface ITenantModuleService
{
    Task AssignModuleAsync(Guid tenantId, Guid moduleId, string? config);
    Task RemoveModuleFromTenantAsync(Guid tenantId, Guid moduleId);
    Task<IEnumerable<TenantModule>> GetTenantModulesAsync(Guid tenantId);
}
