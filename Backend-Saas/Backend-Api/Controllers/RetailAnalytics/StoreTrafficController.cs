using Backend.Application.RetailAnalytics.Models;
using Backend.Application.RetailAnalytics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/retail-analytics/store-traffic")]
    [Authorize]
    [RequireSaasAccess("retail-analytics", "conteo-personas", "sensores-camaras")]
    public class StoreTrafficController : ControllerBase
    {
        private readonly IRetailAnalyticsService _analytics;

        public StoreTrafficController(IRetailAnalyticsService analytics)
        {
            _analytics = analytics;
        }

        [HttpGet]
        [RequirePermission("retail-analytics.conteo-personas.sensores-camaras.ver")]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            return Ok(await _analytics.GetTrafficReadingsAsync(from, to));
        }

        [HttpPost]
        [RequirePermission("retail-analytics.conteo-personas.sensores-camaras.administrar")]
        public async Task<IActionResult> Register([FromBody] StoreTrafficReadingRequest request)
        {
            return Ok(await _analytics.RegisterTrafficReadingAsync(request));
        }
    }
}
