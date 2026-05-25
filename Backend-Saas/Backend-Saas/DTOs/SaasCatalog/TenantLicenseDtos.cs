namespace Backend_Saas.DTOs.SaasCatalog;

public record EnableTenantLicenseRequest(string? Source, DateTime? ExpiresAt);
