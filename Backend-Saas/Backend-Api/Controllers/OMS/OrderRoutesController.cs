using Backend.Application.OMS.Models;
using Backend.Application.OMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.OMS
{
    [ApiController]
    [Route("api/oms/order-routes")]
    [Authorize]
    [RequireSaasAccess("oms", "enrutamiento")]
    public class OrderRoutesController : ControllerBase
    {
        private readonly IOmsRoutingService _routing;

        public OrderRoutesController(IOmsRoutingService routing)
        {
            _routing = routing;
        }

        [HttpGet]
        [RequirePermission("oms.enrutamiento.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _routing.GetOrderRoutesAsync());
        }

        [HttpGet("order/{orderId:guid}")]
        [RequirePermission("oms.enrutamiento.ver")]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            return Ok(await _routing.GetRoutesByOrderAsync(orderId));
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("oms.enrutamiento.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var route = await _routing.GetOrderRouteByIdAsync(id);
            return route is null ? NotFound() : Ok(route);
        }

        [HttpPost]
        [RequirePermission("oms.enrutamiento.administrar")]
        public async Task<IActionResult> Create([FromBody] OrderRouteRequest request)
        {
            return Ok(await _routing.CreateOrderRouteAsync(request));
        }

        [HttpPost("{id:guid}/optimize")]
        [RequirePermission("oms.enrutamiento.administrar")]
        public async Task<IActionResult> Optimize(Guid id)
        {
            var route = await _routing.OptimizeRouteAsync(id);
            return route is null ? NotFound() : Ok(route);
        }

        [HttpPost("{id:guid}/assign")]
        [RequirePermission("oms.enrutamiento.administrar")]
        public async Task<IActionResult> Assign(Guid id)
        {
            var route = await _routing.AssignRouteAsync(id);
            return route is null ? NotFound() : Ok(route);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("oms.enrutamiento.administrar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cancelled = await _routing.CancelRouteAsync(id);
            return cancelled ? NoContent() : NotFound();
        }
    }
}
