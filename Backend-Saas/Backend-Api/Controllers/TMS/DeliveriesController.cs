using Backend.Application.TMS.Models;
using Backend.Application.TMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.TMS
{
    [ApiController]
    [Route("api/tms/deliveries")]
    [Authorize]
    [RequireSaasAccess("tms", "entregas")]
    public class DeliveriesController : ControllerBase
    {
        private readonly ITmsDeliveryService _deliveries;

        public DeliveriesController(ITmsDeliveryService deliveries)
        {
            _deliveries = deliveries;
        }

        [HttpGet]
        [RequirePermission("tms.entregas.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _deliveries.GetDeliveriesAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("tms.entregas.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var delivery = await _deliveries.GetDeliveryByIdAsync(id);
            return delivery is null ? NotFound() : Ok(delivery);
        }

        [HttpPost]
        [RequirePermission("tms.entregas.administrar")]
        public async Task<IActionResult> Create([FromBody] DeliveryRequest request)
        {
            return Ok(await _deliveries.CreateDeliveryAsync(request));
        }

        [HttpPost("{id:guid}/assign")]
        [RequirePermission("tms.entregas.administrar")]
        public async Task<IActionResult> AssignToRoute(Guid id, [FromQuery] Guid routeId, [FromQuery] Guid vehicleId, [FromQuery] Guid driverId)
        {
            var delivery = await _deliveries.AssignToRouteAsync(id, routeId, vehicleId, driverId);
            return delivery is null ? NotFound() : Ok(delivery);
        }

        [HttpPost("{id:guid}/delivered")]
        [RequirePermission("tms.entregas.ejecutar")]
        public async Task<IActionResult> MarkAsDelivered(Guid id)
        {
            var delivery = await _deliveries.MarkAsDeliveredAsync(id);
            return delivery is null ? NotFound() : Ok(delivery);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("tms.entregas.administrar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cancelled = await _deliveries.CancelDeliveryAsync(id);
            return cancelled ? NoContent() : NotFound();
        }
    }
}
