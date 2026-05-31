using Backend.Application.HelpDesk.Models;
using Backend.Application.HelpDesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/help-desk/tickets")]
    [Authorize]
    [RequireSaasAccess("help-desk", "tickets", "reclamos-omnicanal")]
    public class HelpDeskTicketsController : ControllerBase
    {
        private readonly IHelpDeskTicketService _tickets;

        public HelpDeskTicketsController(IHelpDeskTicketService tickets)
        {
            _tickets = tickets;
        }

        [HttpGet]
        [RequirePermission("help-desk.tickets.reclamos-omnicanal.ver")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tickets.GetTicketsAsync());
        }

        [HttpPost]
        [RequirePermission("help-desk.tickets.reclamos-omnicanal.administrar")]
        public async Task<IActionResult> Create([FromBody] HelpDeskTicketRequest request)
        {
            return Ok(await _tickets.CreateTicketAsync(request));
        }

        [HttpPatch("{id:guid}/status")]
        [RequirePermission("help-desk.tickets.reclamos-omnicanal.administrar")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] HelpDeskTicketStatusRequest request)
        {
            var ticket = await _tickets.UpdateStatusAsync(id, request);
            return ticket is null ? NotFound() : Ok(ticket);
        }
    }
}
