using Backend.Application.HelpDesk.Models;

namespace Backend.Application.HelpDesk.Services;

public interface IHelpDeskTicketService
{
    Task<IEnumerable<HelpDeskTicketResponse>> GetTicketsAsync();
    Task<HelpDeskTicketResponse> CreateTicketAsync(HelpDeskTicketRequest request);
    Task<HelpDeskTicketResponse?> UpdateStatusAsync(Guid id, HelpDeskTicketStatusRequest request);
}
