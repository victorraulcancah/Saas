using Backend.Application.POS.Models;
using Backend.Application.POS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/pos/sales")]
    [Authorize]
    [RequireSaasAccess("pos", "caja-atencion", "apertura-cierre-caja")]
    public class SalesController : ControllerBase
    {
        private readonly IPosSalesService _sales;

        public SalesController(IPosSalesService sales)
        {
            _sales = sales;
        }

        [HttpGet]
        [RequirePermission("pos.caja-atencion.apertura-cierre-caja.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _sales.GetSalesAsync());
        }

        [HttpPost]
        [RequirePermission("pos.caja-atencion.apertura-cierre-caja.administrar")]
        public async Task<IActionResult> Create([FromBody] PosSaleRequest request)
        {
            return Ok(await _sales.CreateSaleAsync(request));
        }

        [HttpPost("{id:guid}/complete")]
        [RequirePermission("pos.caja-atencion.apertura-cierre-caja.administrar")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var sale = await _sales.CompleteSaleAsync(id);
            return sale is null ? NotFound() : Ok(sale);
        }

        [HttpPost("{id:guid}/cancel")]
        [RequirePermission("pos.caja-atencion.apertura-cierre-caja.administrar")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var sale = await _sales.CancelSaleAsync(id);
            return sale is null ? NotFound() : Ok(sale);
        }
    }
}
