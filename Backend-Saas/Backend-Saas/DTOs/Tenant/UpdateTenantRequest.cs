namespace Backend_Saas.DTOs.Tenant;

public record UpdateTenantRequest(string Name, string Slug, string? Email, string? SubscriptionPlan, bool IsActive);
