using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/goods-receipts")]
    [Authorize]
    [RequireSaasAccess("erp", "compras-proveedores", "recepcion-mercaderia")]
    public class GoodsReceiptsController : ControllerBase
    {
        private readonly IErpInventoryService _inventory;

        public GoodsReceiptsController(IErpInventoryService inventory)
        {
            _inventory = inventory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _inventory.GetGoodsReceiptsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GoodsReceiptRequest request)
        {
            return Ok(await _inventory.CreateGoodsReceiptAsync(request));
        }

        [HttpPost("{id:guid}/post")]
        public async Task<IActionResult> Post(Guid id)
        {
            var receipt = await _inventory.PostGoodsReceiptAsync(id);
            return receipt is null ? NotFound() : Ok(receipt);
        }
    }
}