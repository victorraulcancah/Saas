using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/warehouse-transfers")]
    [Authorize]
    [RequireSaasAccess("erp", "inventario", "almacenes")]
    public class WarehouseTransfersController : ControllerBase
    {
        private readonly IErpInventoryService _inventory;

        public WarehouseTransfersController(IErpInventoryService inventory)
        {
            _inventory = inventory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _inventory.GetWarehouseTransfersAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WarehouseTransferRequest request)
        {
            return Ok(await _inventory.CreateWarehouseTransferAsync(request));
        }

        [HttpPost("{id:guid}/send")]
        public async Task<IActionResult> Send(Guid id)
        {
            var transfer = await _inventory.SendWarehouseTransferAsync(id);
            return transfer is null ? NotFound() : Ok(transfer);
        }

        [HttpPost("{id:guid}/receive")]
        public async Task<IActionResult> Receive(Guid id)
        {
            var transfer = await _inventory.ReceiveWarehouseTransferAsync(id);
            return transfer is null ? NotFound() : Ok(transfer);
        }
    }
}