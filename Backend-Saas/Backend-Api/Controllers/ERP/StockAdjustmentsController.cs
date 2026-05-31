using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/stock-adjustments")]
    [Authorize]
    [RequireSaasAccess("erp", "inventario", "kardex")]
    public class StockAdjustmentsController : ControllerBase
    {
        private readonly IErpInventoryService _inventory;

        public StockAdjustmentsController(IErpInventoryService inventory)
        {
            _inventory = inventory;
        }

        [HttpGet]
        [RequirePermission("erp.inventario.kardex.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _inventory.GetStockAdjustmentsAsync());
        }

        [HttpPost]
        [RequirePermission("erp.inventario.kardex.administrar")]
        public async Task<IActionResult> Create([FromBody] StockAdjustmentRequest request)
        {
            return Ok(await _inventory.CreateStockAdjustmentAsync(request));
        }
    }
}
