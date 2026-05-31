using Backend.Application.CRM.Models;
using Backend.Application.CRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/crm/sales-orders")]
    [Authorize]
    [RequireSaasAccess("crm", "ventas-b2b", "cotizaciones")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ICrmSalesOrderService _orders;

        public SalesOrdersController(ICrmSalesOrderService orders)
        {
            _orders = orders;
        }

        [HttpGet]
        [RequirePermission("crm.ventas-b2b.cotizaciones.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orders.GetSalesOrdersAsync());
        }

        [HttpPost]
        [RequirePermission("crm.ventas-b2b.cotizaciones.administrar")]
        public async Task<IActionResult> Create([FromBody] SalesOrderRequest request)
        {
            return Ok(await _orders.CreateSalesOrderAsync(request));
        }

        [HttpPost("{id:guid}/confirm")]
        [RequirePermission("crm.ventas-b2b.cotizaciones.administrar")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var order = await _orders.ConfirmSalesOrderAsync(id);
            return order is null ? NotFound() : Ok(order);
        }
    }
}
