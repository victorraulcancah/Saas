using Backend.Application.OMS.Models;
using Backend.Application.OMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/oms/orders")]
    [Authorize]
    [RequireSaasAccess("oms", "enrutamiento-inteligente", "asignacion-stock")]
    public class OmnichannelOrdersController : ControllerBase
    {
        private readonly IOmsOrderService _orders;

        public OmnichannelOrdersController(IOmsOrderService orders)
        {
            _orders = orders;
        }

        [HttpGet]
        [RequirePermission("oms.enrutamiento-inteligente.asignacion-stock.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orders.GetOrdersAsync());
        }

        [HttpPost]
        [RequirePermission("oms.enrutamiento-inteligente.asignacion-stock.administrar")]
        public async Task<IActionResult> Create([FromBody] OmnichannelOrderRequest request)
        {
            return Ok(await _orders.CreateOrderAsync(request));
        }

        [HttpPost("{id:guid}/fulfillment")]
        [RequirePermission("oms.enrutamiento-inteligente.asignacion-stock.administrar")]
        public async Task<IActionResult> AssignFulfillment(Guid id, [FromBody] FulfillmentAssignmentRequest request)
        {
            var order = await _orders.AssignFulfillmentAsync(id, request);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("oms.enrutamiento-inteligente.asignacion-stock.administrar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var order = await _orders.CancelOrderAsync(id);
            return order is null ? NotFound() : Ok(order);
        }
    }
}
