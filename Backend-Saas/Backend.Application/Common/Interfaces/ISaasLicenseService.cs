namespace Backend.Application.Common.Interfaces;

public interface ISaasLicenseService
{
    Task EnableSystemAsync(Guid tenantId, Guid systemId, string source = "Manual", DateTime? expiresAt = null);
    Task EnableModuleAsync(Guid tenantId, Guid moduleId, string source = "Manual", DateTime? expiresAt = null);
    Task EnableSubModuleAsync(Guid tenantId, Guid subModuleId, string source = "Manual", DateTime? expiresAt = null);
    Task DisableSystemAsync(Guid tenantId, Guid systemId);
    Task DisableModuleAsync(Guid tenantId, Guid moduleId);
    Task DisableSubModuleAsync(Guid tenantId, Guid subModuleId);
}
