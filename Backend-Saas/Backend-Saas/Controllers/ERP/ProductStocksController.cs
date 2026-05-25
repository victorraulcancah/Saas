using Backend.Application.ERP.Services;
using Backend_Saas.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.ERP;

[ApiController]
[Route("api/erp/product-stocks")]
[Authorize]
[RequireSaasAccess("erp", "inventario", "kardex")]
public class ProductStocksController : ControllerBase
{
    private readonly IErpCatalogService _catalog;

    public ProductStocksController(IErpCatalogService catalog)
    {
        _catalog = catalog;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? productId, [FromQuery] Guid? warehouseId)
    {
        return Ok(await _catalog.GetProductStocksAsync(productId, warehouseId));
    }
}
