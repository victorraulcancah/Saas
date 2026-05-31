using Backend.Application.CRM.Models;
using Backend.Application.CRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/crm/customers")]
    [Authorize]
    [RequireSaasAccess("crm", "ventas-b2b", "pipeline")]
    public class CustomersController : ControllerBase
    {
        private readonly ICrmCustomerService _customers;

        public CustomersController(ICrmCustomerService customers)
        {
            _customers = customers;
        }

        [HttpGet]
        [RequirePermission("crm.ventas-b2b.pipeline.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _customers.GetCustomersAsync());
        }

        [HttpPost]
        [RequirePermission("crm.ventas-b2b.pipeline.administrar")]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            return Ok(await _customers.CreateCustomerAsync(request));
        }

        [HttpPut("{id:guid}")]
        [RequirePermission("crm.ventas-b2b.pipeline.administrar")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerRequest request)
        {
            var customer = await _customers.UpdateCustomerAsync(id, request);
            return customer is null ? NotFound() : Ok(customer);
        }
    }
}
