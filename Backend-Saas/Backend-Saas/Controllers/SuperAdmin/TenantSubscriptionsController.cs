using Backend.Application.Common.Interfaces;
using Backend.Domain.Saas.Entities;
using Backend_Saas.DTOs.SaasCatalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.SuperAdmin;

[ApiController]
[Route("api/superadmin/tenants/{tenantId:guid}/subscriptions")]
[Authorize(Roles = "SuperAdmin")]
public class TenantSubscriptionsController : ControllerBase
{
    private readonly ISaasSubscriptionService _subscriptionService;

    public TenantSubscriptionsController(ISaasSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpPost("assign-plan")]
    public async Task<IActionResult> AssignPlan(Guid tenantId, [FromBody] AssignTenantPlanRequest request)
    {
        var subscription = await _subscriptionService.AssignPlanAsync(
            tenantId,
            request.PlanId,
            request.Status ?? "Active",
            request.TrialEndsAt,
            request.CurrentPeriodEndsAt);

        return Ok(MapSubscription(subscription));
    }

    private static TenantSubscriptionResponse MapSubscription(TenantSubscription subscription) => new(
        subscription.Id,
        subscription.TenantId,
        subscription.PlanId,
        subscription.Status,
        subscription.StartsAt,
        subscription.TrialEndsAt,
        subscription.CurrentPeriodEndsAt
    );
}
