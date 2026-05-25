namespace Backend_Saas.DTOs.Tenant;

public record ProvisionRequest(string Email, string Password, string? FirstName, string? LastName);
