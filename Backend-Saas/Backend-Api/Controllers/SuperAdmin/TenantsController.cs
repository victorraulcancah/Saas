using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/superadmin/[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly ITenantUserService _tenantUserService;

        public TenantsController(ITenantService tenantService, ITenantUserService tenantUserService)
        {
            _tenantService = tenantService;
            _tenantUserService = tenantUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            var response = tenants.Select(MapResponse);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant is null) return NotFound();
            return Ok(MapResponse(tenant));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DTOs.Tenant.CreateTenantApiRequest request)
        {
            var tenant = await _tenantService.CreateTenantAsync(new CreateTenantRequest(
                request.Name, request.Slug, request.Ruc, request.RazonSocial,
                request.NombreComercial, request.Email, request.EmailFacturacion,
                request.Phone, request.TelefonoSecundario, request.Address,
                request.DireccionFiscal, request.Ubigeo, request.Departamento,
                request.Provincia, request.Distrito, request.Website,
                request.LogoBase64, request.ClaveSol, request.CertificadoPem,
                request.CertificadoPassword, request.SubscriptionPlan
            ));
            return Ok(MapResponse(tenant));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DTOs.Tenant.UpdateTenantApiRequest request)
        {
            var tenant = await _tenantService.UpdateTenantAsync(id, new UpdateTenantRequest(
                request.Name, request.Slug, request.Ruc, request.RazonSocial,
                request.NombreComercial, request.Email, request.EmailFacturacion,
                request.Phone, request.TelefonoSecundario, request.Address,
                request.DireccionFiscal, request.Ubigeo, request.Departamento,
                request.Provincia, request.Distrito, request.Website,
                request.LogoBase64, request.ClaveSol, request.CertificadoPem,
                request.CertificadoPassword, request.SubscriptionPlan,
                request.IsActive
            ));
            return Ok(MapResponse(tenant));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _tenantService.DeleteTenantAsync(id);
            return Ok(new { message = "Tenant desactivado correctamente" });
        }

        [HttpPost("{tenantId:guid}/users")]
        public async Task<IActionResult> CreateUser(Guid tenantId, [FromBody] DTOs.Tenant.CreateTenantUserRequest request)
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
        public async Task<IActionResult> Provision(Guid tenantId, [FromBody] DTOs.Tenant.ProvisionRequest request)
        {
            var user = await _tenantUserService.CreateTenantUserAsync(tenantId, request.Email, request.Password, request.FirstName, request.LastName);
            return Ok(new
            {
                message = "Tenant aprovisionado. Asigna un plan desde /api/superadmin/tenants/{tenantId}/subscriptions/assign-plan para habilitar sistemas, módulos y submódulos.",
                userId = user.Id
            });
        }

        private static DTOs.Tenant.TenantResponse MapResponse(Tenant t) => new(
            t.Id, t.Name, t.Slug, t.Ruc, t.RazonSocial, t.NombreComercial,
            t.Email, t.EmailFacturacion, t.Phone, t.TelefonoSecundario,
            t.Address, t.DireccionFiscal, t.Ubigeo, t.Departamento,
            t.Provincia, t.Distrito, t.Website, t.LogoBase64,
            t.IsActive, t.SubscriptionPlan, t.CreatedAt
        );
    }
}