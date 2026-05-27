using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Backend_Saas.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.ERP;

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
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _inventory.GetMovementsAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryMovementRequest request)
    {
        return Ok(await _inventory.CreateMovementAsync(request));
    }
}
