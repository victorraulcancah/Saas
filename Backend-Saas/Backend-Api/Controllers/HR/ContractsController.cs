using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.HR
{
    [ApiController]
    [Route("api/hr/contracts")]
    [Authorize]
    [RequireSaasAccess("rh", "contratos")]
    public class ContractsController : ControllerBase
    {
        private readonly IHrContractService _contracts;

        public ContractsController(IHrContractService contracts)
        {
            _contracts = contracts;
        }

        [HttpGet]
        [RequirePermission("rh.contratos.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _contracts.GetContractsAsync());
        }

        [HttpGet("employee/{employeeId:guid}")]
        [RequirePermission("rh.contratos.ver")]
        public async Task<IActionResult> GetByEmployee(Guid employeeId)
        {
            return Ok(await _contracts.GetContractsByEmployeeAsync(employeeId));
        }

        [HttpGet("{id:guid}")]
        [RequirePermission("rh.contratos.ver")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contract = await _contracts.GetContractByIdAsync(id);
            return contract is null ? NotFound() : Ok(contract);
        }

        [HttpPost]
        [RequirePermission("rh.contratos.administrar")]
        public async Task<IActionResult> Create([FromBody] ContractRequest request)
        {
            return Ok(await _contracts.CreateContractAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("rh.contratos.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ContractRequest request)
        {
            var contract = await _contracts.UpdateContractAsync(id, request);
            return contract is null ? NotFound() : Ok(contract);
        }

        [HttpPost("{id:guid}/terminate")]
        [RequirePermission("rh.contratos.administrar")]
        public async Task<IActionResult> Terminate(Guid id, [FromBody] string reason)
        {
            var contract = await _contracts.TerminateContractAsync(id, reason);
            return contract is null ? NotFound() : Ok(contract);
        }

        [HttpDelete("{id:guid}")]
        [RequirePermission("rh.contratos.administrar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _contracts.DeleteContractAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
