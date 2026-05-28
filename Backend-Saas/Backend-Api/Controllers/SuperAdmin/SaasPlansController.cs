using Backend.Application.Common.Interfaces;
using Backend.Domain.Saas.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
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

        private static DTOs.SaasCatalog.SaasPlanResponse MapPlan(SaasPlan plan) => new(
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
}