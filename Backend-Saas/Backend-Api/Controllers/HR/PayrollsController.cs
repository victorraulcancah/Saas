using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/hr/payrolls")]
    [Authorize]
    [RequireSaasAccess("rh", "nomina", "sueldos-horas-extra")]
    public class PayrollsController : ControllerBase
    {
        private readonly IHrPayrollService _payrolls;

        public PayrollsController(IHrPayrollService payrolls)
        {
            _payrolls = payrolls;
        }

        [HttpGet]
        [RequirePermission("rh.nomina.sueldos-horas-extra.ver")]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            return Ok(await _payrolls.GetPayrollsAsync(from, to));
        }

        [HttpPost("calculate")]
        [RequirePermission("rh.nomina.sueldos-horas-extra.administrar")]
        public async Task<IActionResult> Calculate([FromBody] PayrollRequest request)
        {
            return Ok(await _payrolls.CalculatePayrollAsync(request));
        }

        [HttpPost("{id:guid}/pay")]
        [RequirePermission("rh.nomina.sueldos-horas-extra.administrar")]
        public async Task<IActionResult> Pay(Guid id, [FromBody] PayPayrollRequest? request)
        {
            var payroll = await _payrolls.MarkAsPaidAsync(id, request?.PaymentDate);
            return payroll is null ? NotFound() : Ok(payroll);
        }
    }

    public record PayPayrollRequest(DateTime? PaymentDate);
}
