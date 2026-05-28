namespace Backend_Api.DTOs.User
{
    public record CreateUserRequest(string Email, string Password, string? FirstName, string? LastName, List<Guid>? RoleIds);
}