namespace Backend_Saas.DTOs.Tenant;

public record CreateTenantUserRequest(string Email, string Password, string? FirstName, string? LastName);
