namespace Backend_Saas.DTOs.Module;

public record CreateModuleRequest(string Name, string Key, string? Description, string? Icon);
