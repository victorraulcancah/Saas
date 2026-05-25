namespace Backend_Saas.DTOs.SaasCatalog;

public record CreateSaasSystemRequest(string Name, string Key, string? Description, string? Icon, string? BasePath);
public record CreateSaasModuleRequest(Guid SystemId, string Name, string Key, string? Description, string? Icon, string? BasePath);
public record CreateSaasSubModuleRequest(Guid ModuleId, string Name, string Key, string? Description, string? RoutePath);

public record SaasSubModuleResponse(Guid Id, string Name, string Key, string? Description, string? RoutePath, bool IsActive);
public record SaasModuleResponse(Guid Id, string Name, string Key, string? Description, string? Icon, string? BasePath, bool IsActive, IEnumerable<SaasSubModuleResponse> SubModules);
public record SaasSystemResponse(Guid Id, string Name, string Key, string? Description, string? Icon, string? BasePath, bool IsActive, IEnumerable<SaasModuleResponse> Modules);
