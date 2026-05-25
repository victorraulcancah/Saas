namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Saas.Entities;

public interface ISaasSubscriptionService
{
    Task<IEnumerable<SaasPlan>> GetPlansAsync();
    Task<TenantSubscription> AssignPlanAsync(Guid tenantId, Guid planId, string status = "Active", DateTime? trialEndsAt = null, DateTime? currentPeriodEndsAt = null);
}
