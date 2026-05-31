using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.HR
{
    [ApiController]
    [Route("api/hr/commissions")]
    [Authorize]
    [RequireSaasAccess("rh", "nomina", "comisiones")]
    public class CommissionsController : ControllerBase
    {
        private readonly IHrCommissionService _commissions;

        public CommissionsController(IHrCommissionService commissions)
        {
            _commissions = commissions;
        }

        [HttpGet]
        [RequirePermission("rh.nomina.comisiones.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _commissions.GetCommissionsAsync());
        }

        [HttpGet("employee/{employeeId:guid}")]
        [RequirePermission("rh.nomina.comisiones.ver")]
        public async Task<IActionResult> GetByEmployee(Guid employeeId)
        {
            return Ok(await _commissions.GetCommissionsByEmployeeAsync(employeeId));
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("rh.nomina.comisiones.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var commission = await _commissions.GetCommissionByIdAsync(id);
            return commission is null ? NotFound() : Ok(commission);
        }

        [HttpPost]
        [RequirePermission("rh.nomina.comisiones.administrar")]
        public async Task<IActionResult> Create([FromBody] CommissionRequest request)
        {
            return Ok(await _commissions.CreateCommissionAsync(request));
        }

        [HttpPost("{id:guid}/mark-paid")]
        [RequirePermission("rh.nomina.comisiones.administrar")]
        public async Task<IActionResult> MarkAsPaid(Guid id)
        {
            var commission = await _commissions.MarkAsPaidAsync(id);
            return commission is null ? NotFound() : Ok(commission);
        }

        [HttpDelete("{id:guid}")]
        [RequirePermission("rh.nomina.comisiones.administrar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _commissions.DeleteCommissionAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
