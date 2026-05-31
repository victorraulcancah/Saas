using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/suppliers")]
    [Authorize]
    [RequireSaasAccess("erp", "compras-proveedores", "proveedores")]
    public class SuppliersController : ControllerBase
    {
        private readonly IErpCatalogService _catalog;

        public SuppliersController(IErpCatalogService catalog)
        {
            _catalog = catalog;
        }

        [HttpGet]
        [RequirePermission("erp.compras-proveedores.proveedores.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _catalog.GetSuppliersAsync());
        }

        [HttpPost]
        [RequirePermission("erp.compras-proveedores.proveedores.administrar")]
        public async Task<IActionResult> Create([FromBody] SupplierRequest request)
        {
            return Ok(await _catalog.CreateSupplierAsync(request));
        }
    }
}
