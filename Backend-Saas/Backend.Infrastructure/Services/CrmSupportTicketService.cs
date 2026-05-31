using Backend.Application.CRM.Models;
using Backend.Application.CRM.Services;
using Backend.Domain.CRM.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class CrmSupportTicketService : ICrmSupportTicketService
{
    private readonly AppDbContext _db;

    public CrmSupportTicketService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<SupportTicketResponse>> GetTicketsAsync() =>
        (await _db.SupportTickets.AsNoTracking().Include(t => t.Customer).OrderByDescending(t => t.CreatedAt).ToListAsync()).Select(Map);

    public async Task<SupportTicketResponse> CreateTicketAsync(SupportTicketRequest request)
    {
        var customer = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CustomerId)
            ?? throw new KeyNotFoundException("Cliente no encontrado");

        var ticket = new SupportTicket
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Subject = request.Subject,
            Description = request.Description ?? string.Empty,
            Priority = request.Priority,
            Status = SupportTicket.TicketStatus.Open,
            AssignedTo = request.AssignedTo ?? string.Empty
        };

        _db.SupportTickets.Add(ticket);
        await _db.SaveChangesAsync();
        ticket.Customer = customer;
        return Map(ticket);
    }

    public async Task<SupportTicketResponse?> UpdateStatusAsync(Guid id, SupportTicket.TicketStatus status, string? resolution)
    {
        var ticket = await _db.SupportTickets.Include(t => t.Customer).FirstOrDefaultAsync(t => t.Id == id);
        if (ticket is null) return null;

        ticket.Status = status;
        ticket.Resolution = resolution ?? ticket.Resolution;
        ticket.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(ticket);
    }

    private static SupportTicketResponse Map(SupportTicket t) => new(t.Id, t.CustomerId, t.Customer?.Name ?? string.Empty, t.Subject, t.Description, t.Priority, t.Status, t.AssignedTo, t.Resolution);
}
