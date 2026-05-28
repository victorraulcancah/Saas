namespace Backend_Api.DTOs.Role
{
    public record CreateRoleRequest(string Name, string? Description, List<Guid>? PermissionIds);
}