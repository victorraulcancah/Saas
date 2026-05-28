namespace Backend_Api.DTOs.Tenant
{
    public record ProvisionRequest(string Email, string Password, string? FirstName, string? LastName);
}