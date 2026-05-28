namespace Backend_Api.DTOs.SaasCatalog
{
    public record AssignTenantPlanRequest(Guid PlanId, string? Status, DateTime? TrialEndsAt, DateTime? CurrentPeriodEndsAt);

    public record SaasPlanResponse(Guid Id, string Name, string Key, string? Description, decimal Price, string Currency, string BillingCycle, int MaxUsers, int MaxBranches, int MaxDocumentsPerMonth);

    public record TenantSubscriptionResponse(Guid Id, Guid TenantId, Guid PlanId, string Status, DateTime StartsAt, DateTime? TrialEndsAt, DateTime? CurrentPeriodEndsAt);
}