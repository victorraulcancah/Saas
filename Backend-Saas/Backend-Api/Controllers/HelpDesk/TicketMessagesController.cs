using Backend.Application.HelpDesk.Models;
using Backend.Application.HelpDesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Api.Controllers.HelpDesk
{
    [ApiController]
    [Route("api/helpdesk/ticket-messages")]
    [Authorize]
    [RequireSaasAccess("helpdesk", "mensajes")]
    public class TicketMessagesController : ControllerBase
    {
        private readonly IHelpDeskMessageService _messages;

        public TicketMessagesController(IHelpDeskMessageService messages)
        {
            _messages = messages;
        }

        [HttpGet("ticket/{ticketId:guid}")]
        [RequirePermission("helpdesk.mensajes.ver")]
        public async Task<IActionResult> GetByTicket(Guid ticketId)
        {
            return Ok(await _messages.GetTicketMessagesAsync(ticketId));
        }

        [HttpPost]
        [RequirePermission("helpdesk.mensajes.crear")]
        public async Task<IActionResult> Create([FromBody] TicketMessageRequest request)
        {
            return Ok(await _messages.CreateMessageAsync(request));
        }
    }
}
