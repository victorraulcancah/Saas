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
    private readonly ITenantModuleService _tenantModuleService;
    private readonly ITenantUserService _tenantUserService;

    public TenantsController(ITenantService tenantService, ITenantModuleService tenantModuleService, ITenantUserService tenantUserService)
    {
        _tenantService = tenantService;
        _tenantModuleService = tenantModuleService;
        _tenantUserService = tenantUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tenants = await _tenantService.GetAllTenantsAsync();
        var response = tenants.Select(t => new TenantResponse(t.Id, t.Name, t.Slug, t.Email, t.IsActive, t.SubscriptionPlan, t.CreatedAt));
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var tenant = await _tenantService.GetTenantByIdAsync(id);
        if (tenant is null) return NotFound();
        return Ok(new TenantResponse(tenant.Id, tenant.Name, tenant.Slug, tenant.Email, tenant.IsActive, tenant.SubscriptionPlan, tenant.CreatedAt));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request)
    {
        var tenant = await _tenantService.CreateTenantAsync(request.Name, request.Slug, request.Email, request.SubscriptionPlan);
        return Ok(new TenantResponse(tenant.Id, tenant.Name, tenant.Slug, tenant.Email, tenant.IsActive, tenant.SubscriptionPlan, tenant.CreatedAt));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTenantRequest request)
    {
        var tenant = await _tenantService.UpdateTenantAsync(id, request.Name, request.Slug, request.Email, request.SubscriptionPlan, request.IsActive);
        return Ok(new TenantResponse(tenant.Id, tenant.Name, tenant.Slug, tenant.Email, tenant.IsActive, tenant.SubscriptionPlan, tenant.CreatedAt));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _tenantService.DeleteTenantAsync(id);
        return Ok(new { message = "Tenant desactivado correctamente" });
    }

    [HttpPost("{tenantId:guid}/modules")]
    public async Task<IActionResult> AssignModule(Guid tenantId, [FromBody] AssignModuleRequest request)
    {
        await _tenantModuleService.AssignModuleAsync(tenantId, request.ModuleId, request.Config);
        return Ok(new { message = "Módulo asignado correctamente" });
    }

    [HttpDelete("{tenantId:guid}/modules/{moduleId:guid}")]
    public async Task<IActionResult> RemoveModule(Guid tenantId, Guid moduleId)
    {
        await _tenantModuleService.RemoveModuleFromTenantAsync(tenantId, moduleId);
        return Ok(new { message = "Módulo removido correctamente" });
    }

    [HttpGet("{tenantId:guid}/modules")]
    public async Task<IActionResult> GetModules(Guid tenantId)
    {
        var modules = await _tenantModuleService.GetTenantModulesAsync(tenantId);
        var response = modules.Select(tm => new TenantModuleResponse(tm.ModuleId, tm.Module.Name, tm.Module.Key, tm.IsEnabled, tm.EnabledAt, tm.ExpiresAt));
        return Ok(response);
    }

    [HttpPost("{tenantId:guid}/users")]
    public async Task<IActionResult> CreateUser(Guid tenantId, [FromBody] CreateTenantUserRequest request)
    {
        var user = await _tenantUserService.CreateTenantUserAsync(tenantId, request.Email, request.Password, request.FirstName, request.LastName);
        return Ok(new { userId = user.Id, email = user.Email });
    }

    [HttpGet("{tenantId:guid}/users")]
    public async Task<IActionResult> GetUsers(Guid tenantId)
    {
        var users = await _tenantUserService.GetTenantUsersAsync(tenantId);
        var response = users.Select(u => new { u.Id, u.Email, u.FirstName, u.LastName, IsActive = !u.IsDeleted });
        return Ok(response);
    }

    [HttpPost("{tenantId:guid}/provision")]
    public async Task<IActionResult> Provision(Guid tenantId, [FromBody] ProvisionRequest request)
    {
        var user = await _tenantUserService.CreateTenantUserAsync(tenantId, request.Email, request.Password, request.FirstName, request.LastName);

        if (request.ModuleIds?.Any() == true)
        {
            foreach (var moduleId in request.ModuleIds)
            {
                await _tenantModuleService.AssignModuleAsync(tenantId, moduleId, null);
            }
        }

        return Ok(new { message = "Tenant aprovisionado", userId = user.Id });
    }
}
