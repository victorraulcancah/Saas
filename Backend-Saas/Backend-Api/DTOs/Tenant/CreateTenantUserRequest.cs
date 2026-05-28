namespace Backend_Api.DTOs.Tenant
{
    public record CreateTenantUserRequest(string Email, string Password, string? FirstName, string? LastName);
}