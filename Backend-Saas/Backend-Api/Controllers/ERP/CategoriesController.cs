using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Backend_Saas.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.ERP;

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
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _catalog.GetCategoriesAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryRequest request)
    {
        return Ok(await _catalog.CreateCategoryAsync(request));
    }
}
