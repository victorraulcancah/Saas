using Backend.Application.CRM.Models;
using Backend.Domain.CRM.Entities;

namespace Backend.Application.CRM.Services;

public interface ICrmSupportTicketService
{
    Task<IEnumerable<SupportTicketResponse>> GetTicketsAsync();
    Task<SupportTicketResponse> CreateTicketAsync(SupportTicketRequest request);
    Task<SupportTicketResponse?> UpdateStatusAsync(Guid id, SupportTicket.TicketStatus status, string? resolution);
}
