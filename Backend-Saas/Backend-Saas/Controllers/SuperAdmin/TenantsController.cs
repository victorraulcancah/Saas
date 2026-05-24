using Backend.Application.Common.Interfaces;
using Backend_Saas.DTOs.Tenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Saas.Controllers.SuperAdmin;

[ApiController]
[Route("api/superadmin/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tenants = await _tenantService.GetAllTenantsAsync();
        var response = tenants.Select(t => new TenantResponse(t.Id, t.Name, t.Slug, t.Email, t.IsActive, t.SubscriptionPlan, t.CreatedAt));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request)
    {
        var tenant = await _tenantService.CreateTenantAsync(request.Name, request.Slug, request.Email, request.SubscriptionPlan);
        return Ok(new TenantResponse(tenant.Id, tenant.Name, tenant.Slug, tenant.Email, tenant.IsActive, tenant.SubscriptionPlan, tenant.CreatedAt));
    }

    [HttpPost("{tenantId:guid}/modules")]
    public async Task<IActionResult> AssignModule(Guid tenantId, [FromBody] AssignModuleRequest request)
    {
        await _tenantService.AssignModuleAsync(tenantId, request.ModuleId, request.Config);
        return Ok(new { message = "Módulo asignado correctamente" });
    }
}
