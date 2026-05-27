namespace Backend_Saas.DTOs.Role;

public record CreateRoleRequest(string Name, string? Description, List<Guid>? PermissionIds);
