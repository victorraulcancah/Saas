using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/warehouses")]
    [Authorize]
    [RequireSaasAccess("erp", "inventario", "almacenes")]
    public class WarehousesController : ControllerBase
    {
        private readonly IErpCatalogService _catalog;

        public WarehousesController(IErpCatalogService catalog)
        {
            _catalog = catalog;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _catalog.GetWarehousesAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WarehouseRequest request)
        {
            return Ok(await _catalog.CreateWarehouseAsync(request));
        }
    }
}