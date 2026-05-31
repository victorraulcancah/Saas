using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/hr/employees")]
    [Authorize]
    [RequireSaasAccess("rh", "nomina", "sueldos-horas-extra")]
    public class EmployeesController : ControllerBase
    {
        private readonly IHrEmployeeService _employees;

        public EmployeesController(IHrEmployeeService employees)
        {
            _employees = employees;
        }

        [HttpGet]
        [RequirePermission("rh.nomina.sueldos-horas-extra.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employees.GetEmployeesAsync());
        }

        [HttpPost]
        [RequirePermission("rh.nomina.sueldos-horas-extra.administrar")]
        public async Task<IActionResult> Create([FromBody] EmployeeRequest request)
        {
            return Ok(await _employees.CreateEmployeeAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("rh.nomina.sueldos-horas-extra.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeRequest request)
        {
            var employee = await _employees.UpdateEmployeeAsync(id, request);
            return employee is null ? NotFound() : Ok(employee);
        }

        [HttpPost("{id:guid}/deactivate")]
        [RequirePermission("rh.nomina.sueldos-horas-extra.administrar")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var employee = await _employees.DeactivateEmployeeAsync(id);
            return employee is null ? NotFound() : Ok(employee);
        }
    }
}
