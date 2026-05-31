using Backend.Application.CRM.Models;
using Backend.Application.CRM.Services;
using Backend.Domain.CRM.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/crm/opportunities")]
    [Authorize]
    [RequireSaasAccess("crm", "ventas-b2b", "pipeline")]
    public class OpportunitiesController : ControllerBase
    {
        private readonly ICrmOpportunityService _opportunities;

        public OpportunitiesController(ICrmOpportunityService opportunities)
        {
            _opportunities = opportunities;
        }

        [HttpGet]
        [RequirePermission("crm.ventas-b2b.pipeline.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _opportunities.GetOpportunitiesAsync());
        }

        [HttpPost]
        [RequirePermission("crm.ventas-b2b.pipeline.administrar")]
        public async Task<IActionResult> Create([FromBody] OpportunityRequest request)
        {
            return Ok(await _opportunities.CreateOpportunityAsync(request));
        }

        [HttpPatch("{id:guid}/stage")]
        [RequirePermission("crm.ventas-b2b.pipeline.administrar")]
        public async Task<IActionResult> UpdateStage(Guid id, [FromBody] UpdateOpportunityStageRequest request)
        {
            var opportunity = await _opportunities.UpdateStageAsync(id, request.Stage);
            return opportunity is null ? NotFound() : Ok(opportunity);
        }
    }

    public record UpdateOpportunityStageRequest(Opportunity.OpportunityStage Stage);
}
