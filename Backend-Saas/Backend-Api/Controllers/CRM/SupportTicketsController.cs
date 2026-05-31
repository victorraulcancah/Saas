using Backend.Application.CRM.Models;
using Backend.Application.CRM.Services;
using Backend.Domain.CRM.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api
{
    [ApiController]
    [Route("api/crm/support-tickets")]
    [Authorize]
    [RequireSaasAccess("help-desk", "tickets", "reclamos-omnicanal")]
    public class SupportTicketsController : ControllerBase
    {
        private readonly ICrmSupportTicketService _tickets;

        public SupportTicketsController(ICrmSupportTicketService tickets)
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
        public async Task<IActionResult> Create([FromBody] SupportTicketRequest request)
        {
            return Ok(await _tickets.CreateTicketAsync(request));
        }

        [HttpPatch("{id:guid}/status")]
        [RequirePermission("help-desk.tickets.reclamos-omnicanal.administrar")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateSupportTicketStatusRequest request)
        {
            var ticket = await _tickets.UpdateStatusAsync(id, request.Status, request.Resolution);
            return ticket is null ? NotFound() : Ok(ticket);
        }
    }

    public record UpdateSupportTicketStatusRequest(SupportTicket.TicketStatus Status, string? Resolution);
}
