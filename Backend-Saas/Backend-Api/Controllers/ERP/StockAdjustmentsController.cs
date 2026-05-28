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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _inventory.GetStockAdjustmentsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StockAdjustmentRequest request)
        {
            return Ok(await _inventory.CreateStockAdjustmentAsync(request));
        }
    }
}