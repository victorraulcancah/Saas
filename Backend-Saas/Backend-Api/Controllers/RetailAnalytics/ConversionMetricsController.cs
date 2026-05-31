using Backend.Application.RetailAnalytics.Models;
using Backend.Application.RetailAnalytics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.RetailAnalytics
{
    [ApiController]
    [Route("api/retail-analytics/conversion-metrics")]
    [Authorize]
    [RequireSaasAccess("retail-analytics", "conversion")]
    public class ConversionMetricsController : ControllerBase
    {
        private readonly IRetailConversionService _conversion;

        public ConversionMetricsController(IRetailConversionService conversion)
        {
            _conversion = conversion;
        }

        [HttpGet("branch/{branchId:guid}")]
        [RequirePermission("retail-analytics.conversion.ver")]
        public async Task<IActionResult> GetByBranch(Guid branchId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(await _conversion.GetConversionMetricsAsync(branchId, startDate, endDate));
        }

        [HttpPost]
        [RequirePermission("retail-analytics.conversion.administrar")]
        public async Task<IActionResult> Create([FromBody] ConversionMetricRequest request)
        {
            return Ok(await _conversion.CreateConversionMetricAsync(request));
        }
    }
}
