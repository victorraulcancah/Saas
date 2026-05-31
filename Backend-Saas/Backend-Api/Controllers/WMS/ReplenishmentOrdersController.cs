using Backend.Application.WMS.Models;
using Backend.Application.WMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.WMS
{
    [ApiController]
    [Route("api/wms/replenishment-orders")]
    [Authorize]
    [RequireSaasAccess("wms", "replenishment")]
    public class ReplenishmentOrdersController : ControllerBase
    {
        private readonly IWmsReplenishmentService _replenishment;

        public ReplenishmentOrdersController(IWmsReplenishmentService replenishment)
        {
            _replenishment = replenishment;
        }

        [HttpGet]
        [RequirePermission("wms.replenishment.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _replenishment.GetReplenishmentOrdersAsync());
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("wms.replenishment.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _replenishment.GetReplenishmentOrderByIdAsync(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost]
        [RequirePermission("wms.replenishment.administrar")]
        public async Task<IActionResult> Create([FromBody] ReplenishmentOrderRequest request)
        {
            return Ok(await _replenishment.CreateReplenishmentOrderAsync(request));
        }

        [HttpPost("{id:guid}/approve")]
        [RequirePermission("wms.replenishment.aprobar")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var order = await _replenishment.ApproveReplenishmentOrderAsync(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost("{id:guid}/complete")]
        [RequirePermission("wms.replenishment.administrar")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var order = await _replenishment.CompleteReplenishmentOrderAsync(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("wms.replenishment.administrar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cancelled = await _replenishment.CancelReplenishmentOrderAsync(id);
            return cancelled ? NoContent() : NotFound();
        }
    }
}
