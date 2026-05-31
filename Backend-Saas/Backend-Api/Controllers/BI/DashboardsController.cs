using Backend.Application.BI.Models;
using Backend.Application.BI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.BI
{
    [ApiController]
    [Route("api/bi/dashboards")]
    [Authorize]
    [RequireSaasAccess("bi", "dashboards")]
    public class DashboardsController : ControllerBase
    {
        private readonly IBiDashboardService _dashboards;

        public DashboardsController(IBiDashboardService dashboards)
        {
            _dashboards = dashboards;
        }

        [HttpGet]
        [RequirePermission("bi.dashboards.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dashboards.GetDashboardsAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("bi.dashboards.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dashboard = await _dashboards.GetDashboardByIdAsync(id);
            return dashboard is null ? NotFound() : Ok(dashboard);
        }

        [HttpPost]
        [RequirePermission("bi.dashboards.administrar")]
        public async Task<IActionResult> Create([FromBody] DashboardRequest request)
        {
            return Ok(await _dashboards.CreateDashboardAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("bi.dashboards.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DashboardRequest request)
        {
            var dashboard = await _dashboards.UpdateDashboardAsync(id, request);
            return dashboard is null ? NotFound() : Ok(dashboard);
        }

        [HttpDelete("{id:guid}")]
        [RequirePermission("bi.dashboards.administrar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _dashboards.DeleteDashboardAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
