using Backend.Application.BI.Models;
using Backend.Application.BI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/bi/metrics")]
    [Authorize]
    [RequireSaasAccess("bi", "dashboards", "rentabilidad")]
    public class MetricsController : ControllerBase
    {
        private readonly IBiMetricsService _metrics;

        public MetricsController(IBiMetricsService metrics)
        {
            _metrics = metrics;
        }

        [HttpGet]
        [RequirePermission("bi.dashboards.rentabilidad.ver")]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            return Ok(await _metrics.GetMetricsAsync(from, to));
        }

        [HttpPost]
        [RequirePermission("bi.dashboards.rentabilidad.administrar")]
        public async Task<IActionResult> Create([FromBody] MetricSnapshotRequest request)
        {
            return Ok(await _metrics.CreateMetricSnapshotAsync(request));
        }
    }
}
