namespace Backend_Saas.DTOs.Tenant;

public record TenantResponse(Guid Id, string Name, string Slug, string? Email, bool IsActive, string? SubscriptionPlan, DateTime CreatedAt);
