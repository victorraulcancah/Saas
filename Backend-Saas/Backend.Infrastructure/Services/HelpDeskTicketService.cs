using Backend.Application.HelpDesk.Models;
using Backend.Application.HelpDesk.Services;
using Backend.Domain.HelpDesk.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HelpDeskTicketService : IHelpDeskTicketService
{
    private readonly AppDbContext _db;

    public HelpDeskTicketService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<HelpDeskTicketResponse>> GetTicketsAsync() =>
        (await _db.HelpDeskTickets.AsNoTracking().OrderByDescending(t => t.CreatedAt).ToListAsync()).Select(Map);

    public async Task<HelpDeskTicketResponse> CreateTicketAsync(HelpDeskTicketRequest request)
    {
        var ticket = new HelpDeskTicket
        {
            Id = Guid.NewGuid(),
            TicketNumber = request.TicketNumber,
            Subject = request.Subject,
            Description = request.Description ?? string.Empty,
            Channel = request.Channel ?? string.Empty,
            Priority = request.Priority,
            AssignedTo = request.AssignedTo ?? string.Empty,
            FirstResponseDueAt = request.FirstResponseDueAt,
            ResolutionDueAt = request.ResolutionDueAt
        };

        if (!string.IsNullOrWhiteSpace(ticket.AssignedTo))
            ticket.Status = HelpDeskTicket.HelpDeskStatus.Assigned;

        _db.HelpDeskTickets.Add(ticket);
        await _db.SaveChangesAsync();
        return Map(ticket);
    }

    public async Task<HelpDeskTicketResponse?> UpdateStatusAsync(Guid id, HelpDeskTicketStatusRequest request)
    {
        var ticket = await _db.HelpDeskTickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket is null) return null;

        ticket.Status = request.Status;
        ticket.Resolution = request.Resolution ?? ticket.Resolution;
        ticket.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(ticket);
    }

    private static HelpDeskTicketResponse Map(HelpDeskTicket ticket) => new(ticket.Id, ticket.TicketNumber, ticket.Subject, ticket.Description, ticket.Channel, ticket.Priority, ticket.Status, ticket.AssignedTo, ticket.FirstResponseDueAt, ticket.ResolutionDueAt, ticket.Resolution);
}
