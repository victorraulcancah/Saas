namespace Backend_Saas.DTOs.Tenant;

public record TenantModuleResponse(Guid ModuleId, string ModuleName, string ModuleKey, bool IsEnabled, DateTime EnabledAt, DateTime? ExpiresAt);
