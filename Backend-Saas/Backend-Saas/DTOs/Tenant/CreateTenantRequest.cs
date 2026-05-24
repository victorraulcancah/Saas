namespace Backend_Saas.DTOs.Tenant;

public record CreateTenantRequest(string Name, string Slug, string? Email, string? SubscriptionPlan);
