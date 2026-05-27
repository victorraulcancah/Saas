using Backend.Application.Common.Interfaces;
using Backend.Domain.Saas.Entities;
using Backend_Saas.DTOs.SaasCatalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.SuperAdmin;

[ApiController]
[Route("api/superadmin/saas-plans")]
[Authorize(Roles = "SuperAdmin")]
public class SaasPlansController : ControllerBase
{
    private readonly ISaasSubscriptionService _subscriptionService;

    public SaasPlansController(ISaasSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlans()
    {
        var plans = await _subscriptionService.GetPlansAsync();
        return Ok(plans.Select(MapPlan));
    }

    private static SaasPlanResponse MapPlan(SaasPlan plan) => new(
        plan.Id,
        plan.Name,
        plan.Key,
        plan.Description,
        plan.Price,
        plan.Currency,
        plan.BillingCycle,
        plan.MaxUsers,
        plan.MaxBranches,
        plan.MaxDocumentsPerMonth
    );
}
