using Backend.Application.SFA.Models;
using Backend.Application.SFA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/sfa/field-orders")]
    [Authorize]
    [RequireSaasAccess("sfa", "preventa-movil", "pedidos-ruta")]
    public class FieldOrdersController : ControllerBase
    {
        private readonly ISfaFieldOrderService _orders;

        public FieldOrdersController(ISfaFieldOrderService orders)
        {
            _orders = orders;
        }

        [HttpGet]
        [RequirePermission("sfa.preventa-movil.pedidos-ruta.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orders.GetOrdersAsync());
        }

        [HttpPost]
        [RequirePermission("sfa.preventa-movil.pedidos-ruta.administrar")]
        public async Task<IActionResult> Create([FromBody] FieldOrderRequest request)
        {
            return Ok(await _orders.CreateOrderAsync(request));
        }

        [HttpPatch("{id:guid}/status")]
        [RequirePermission("sfa.preventa-movil.pedidos-ruta.administrar")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] FieldOrderStatusRequest request)
        {
            var order = await _orders.UpdateStatusAsync(id, request);
            return order is null ? NotFound() : Ok(order);
        }
    }
}
