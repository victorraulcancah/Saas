using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/erp/invoices")]
    [Authorize]
    [RequireSaasAccess("erp", "facturacion", "comprobantes")]
    public class InvoicesController : ControllerBase
    {
        private readonly IErpBillingService _billing;

        public InvoicesController(IErpBillingService billing)
        {
            _billing = billing;
        }

        [HttpGet]
        [RequirePermission("erp.facturacion.comprobantes.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _billing.GetInvoicesAsync());
        }

        [HttpPost]
        [RequirePermission("erp.facturacion.comprobantes.administrar")]
        public async Task<IActionResult> Create([FromBody] InvoiceRequest request)
        {
            return Ok(await _billing.CreateInvoiceAsync(request));
        }

        [HttpPost("{id:guid}/mark-sent")]
        [RequirePermission("erp.facturacion.comprobantes.administrar")]
        public async Task<IActionResult> MarkSent(Guid id)
        {
            var invoice = await _billing.MarkInvoiceSentAsync(id);
            return invoice is null ? NotFound() : Ok(invoice);
        }
    }
}
