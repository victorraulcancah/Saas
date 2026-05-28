namespace Backend_Api.DTOs.SaasCatalog
{
    public record EnableTenantLicenseRequest(string? Source, DateTime? ExpiresAt);
}