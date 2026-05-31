using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.HR
{
    [ApiController]
    [Route("api/hr/vacations")]
    [Authorize]
    [RequireSaasAccess("rh", "vacaciones")]
    public class VacationsController : ControllerBase
    {
        private readonly IHrVacationService _vacations;
        private readonly ICurrentUserService _currentUser;

        public VacationsController(IHrVacationService vacations, ICurrentUserService currentUser)
        {
            _vacations = vacations;
            _currentUser = currentUser;
        }

        [HttpGet]
        [RequirePermission("rh.vacaciones.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _vacations.GetVacationsAsync());
        }

        [HttpGet("employee/{employeeId:guid}")]
        [RequirePermission("rh.vacaciones.ver")]
        public async Task<IActionResult> GetByEmployee(Guid employeeId)
        {
            return Ok(await _vacations.GetVacationsByEmployeeAsync(employeeId));
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("rh.vacaciones.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vacation = await _vacations.GetVacationByIdAsync(id);
            return vacation is null ? NotFound() : Ok(vacation);
        }

        [HttpPost]
        [RequirePermission("rh.vacaciones.solicitar")]
        public async Task<IActionResult> Create([FromBody] VacationRequest request)
        {
            return Ok(await _vacations.CreateVacationAsync(request));
        }

        [HttpPost("{id:guid}/approve")]
        [RequirePermission("rh.vacaciones.aprobar")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("Usuario no autenticado");
            var vacation = await _vacations.ApproveVacationAsync(id, userId);
            return vacation is null ? NotFound() : Ok(vacation);
        }

        [HttpPost("{id:guid}/reject")]
        [RequirePermission("rh.vacaciones.aprobar")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] string reason)
        {
            var vacation = await _vacations.RejectVacationAsync(id, reason);
            return vacation is null ? NotFound() : Ok(vacation);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("rh.vacaciones.solicitar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var cancelled = await _vacations.CancelVacationAsync(id);
            return cancelled ? NoContent() : NotFound();
        }
    }
}
