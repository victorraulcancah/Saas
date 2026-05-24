namespace Backend_Saas.DTOs.Role;

public record RoleResponse(Guid Id, string Name, string? Description, DateTime CreatedAt);
