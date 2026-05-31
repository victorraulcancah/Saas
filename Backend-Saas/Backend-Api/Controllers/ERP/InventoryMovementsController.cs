using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/inventory-movements")]
    [Authorize]
    [RequireSaasAccess("erp", "inventario", "kardex")]
    public class InventoryMovementsController : ControllerBase
    {
        private readonly IErpInventoryService _inventory;

        public InventoryMovementsController(IErpInventoryService inventory)
        {
            _inventory = inventory;
        }

        [HttpGet]
        [RequirePermission("erp.inventario.kardex.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _inventory.GetMovementsAsync());
        }

        [HttpPost]
        [RequirePermission("erp.inventario.kardex.administrar")]
        public async Task<IActionResult> Create([FromBody] InventoryMovementRequest request)
        {
            return Ok(await _inventory.CreateMovementAsync(request));
        }
    }
}
