using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/warehouse-locations")]
    [Authorize]
    [RequireSaasAccess("erp", "inventario", "almacenes")]
    public class WarehouseLocationsController : ControllerBase
    {
        private readonly IErpCatalogService _catalog;

        public WarehouseLocationsController(IErpCatalogService catalog)
        {
            _catalog = catalog;
        }

        [HttpGet("warehouse/{warehouseId:guid}")]
        public async Task<IActionResult> GetByWarehouse(Guid warehouseId)
        {
            return Ok(await _catalog.GetWarehouseLocationsAsync(warehouseId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WarehouseLocationRequest request)
        {
            return Ok(await _catalog.CreateWarehouseLocationAsync(request));
        }
    }
}