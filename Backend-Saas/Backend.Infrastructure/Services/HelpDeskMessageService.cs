using Backend.Application.HelpDesk.Models;
using Backend.Application.HelpDesk.Services;
using Backend.Domain.HelpDesk.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HelpDeskMessageService : IHelpDeskMessageService
{
    private readonly AppDbContext _db;

    public HelpDeskMessageService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TicketMessageResponse>> GetTicketMessagesAsync(Guid ticketId) =>
        (await _db.TicketMessages.AsNoTracking().Where(m => m.HelpDeskTicketId == ticketId).OrderBy(m => m.CreatedAt).ToListAsync()).Select(Map);

    public async Task<TicketMessageResponse> CreateMessageAsync(TicketMessageRequest request)
    {
        var message = new TicketMessage
        {
            Id = Guid.NewGuid(),
            HelpDeskTicketId = request.HelpDeskTicketId,
            Type = request.Type,
            Message = request.Message,
            IsInternal = request.IsInternal,
            UserName = "System"
        };

        _db.TicketMessages.Add(message);
        await _db.SaveChangesAsync();
        return Map(message);
    }

    private static TicketMessageResponse Map(TicketMessage m) =>
        new TicketMessageResponse(m.Id, m.HelpDeskTicketId, m.Type, m.UserId, m.UserName, m.Message, m.IsInternal, m.CreatedAt);
}
