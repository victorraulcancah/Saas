using Backend.Application.HelpDesk.Models;

namespace Backend.Application.HelpDesk.Services;

public interface IHelpDeskMessageService
{
    Task<IEnumerable<TicketMessageResponse>> GetTicketMessagesAsync(Guid ticketId);
    Task<TicketMessageResponse> CreateMessageAsync(TicketMessageRequest request);
}
