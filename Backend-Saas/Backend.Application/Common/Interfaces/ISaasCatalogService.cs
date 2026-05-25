namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Saas.Entities;

public interface ISaasCatalogService
{
    Task<IEnumerable<SaasSystem>> GetCatalogAsync();
    Task<SaasSystem> CreateSystemAsync(string name, string key, string? description, string? icon, string? basePath);
    Task<SaasModule> CreateModuleAsync(Guid systemId, string name, string key, string? description, string? icon, string? basePath);
    Task<SaasSubModule> CreateSubModuleAsync(Guid moduleId, string name, string key, string? description, string? routePath);
}
