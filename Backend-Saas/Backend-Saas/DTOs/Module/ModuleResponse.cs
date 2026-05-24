namespace Backend_Saas.DTOs.Module;

public record ModuleResponse(Guid Id, string Name, string Key, string? Description, string? Icon);
