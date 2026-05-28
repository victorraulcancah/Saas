using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/purchase-orders")]
    [Authorize]
    [RequireSaasAccess("erp", "compras-proveedores", "ordenes-compra")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IErpPurchasingService _purchasing;

        public PurchaseOrdersController(IErpPurchasingService purchasing)
        {
            _purchasing = purchasing;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _purchasing.GetPurchaseOrdersAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseOrderRequest request)
        {
            return Ok(await _purchasing.CreatePurchaseOrderAsync(request));
        }

        [HttpPost("{id:guid}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var order = await _purchasing.ApprovePurchaseOrderAsync(id);
            return order is null ? NotFound() : Ok(order);
        }
    }
}