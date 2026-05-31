using Backend.Application.TMS.Models;
using Backend.Application.TMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/tms/routes")]
    [Authorize]
    [RequireSaasAccess("tms", "planificacion-rutas", "optimizacion-recorridos")]
    public class DeliveryRoutesController : ControllerBase
    {
        private readonly ITmsRouteService _routes;

        public DeliveryRoutesController(ITmsRouteService routes)
        {
            _routes = routes;
        }

        [HttpGet]
        [RequirePermission("tms.planificacion-rutas.optimizacion-recorridos.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _routes.GetRoutesAsync());
        }

        [HttpPost]
        [RequirePermission("tms.planificacion-rutas.optimizacion-recorridos.administrar")]
        public async Task<IActionResult> Create([FromBody] DeliveryRouteRequest request)
        {
            return Ok(await _routes.CreateRouteAsync(request));
        }

        [HttpPatch("{id:guid}/status")]
        [RequirePermission("tms.planificacion-rutas.optimizacion-recorridos.administrar")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] DeliveryRouteStatusRequest request)
        {
            var route = await _routes.UpdateStatusAsync(id, request);
            return route is null ? NotFound() : Ok(route);
        }
    }
}
