using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/products")]
    [Authorize]
    [RequireSaasAccess("erp", "inventario", "productos")]
    public class ProductsController : ControllerBase
    {
        private readonly IErpCatalogService _catalog;

        public ProductsController(IErpCatalogService catalog)
        {
            _catalog = catalog;
        }

        [HttpGet]
        [RequirePermission("erp.inventario.productos.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _catalog.GetProductsAsync());
        }

        [HttpPost]
        [RequirePermission("erp.inventario.productos.administrar")]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            return Ok(await _catalog.CreateProductAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("erp.inventario.productos.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductRequest request)
        {
            var product = await _catalog.UpdateProductAsync(id, request);
            return product is null ? NotFound() : Ok(product);
        }
    }
}
