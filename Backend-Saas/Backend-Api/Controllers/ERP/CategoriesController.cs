using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/categories")]
    [Authorize]
    [RequireSaasAccess("erp", "inventario", "productos")]
    public class CategoriesController : ControllerBase
    {
        private readonly IErpCatalogService _catalog;

        public CategoriesController(IErpCatalogService catalog)
        {
            _catalog = catalog;
        }

        [HttpGet]
        [RequirePermission("erp.inventario.productos.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _catalog.GetCategoriesAsync());
        }

        [HttpPost]
        [RequirePermission("erp.inventario.productos.administrar")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest request)
        {
            return Ok(await _catalog.CreateCategoryAsync(request));
        }
    }
}
