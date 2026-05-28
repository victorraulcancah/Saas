namespace Backend_Api.DTOs.Role
{
    public record RoleResponse(Guid Id, string Name, string? Description, DateTime CreatedAt);
}