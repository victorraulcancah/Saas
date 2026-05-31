using Backend.Application.CRM.Models;
using Backend.Application.CRM.Services;
using Backend.Domain.CRM.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class CrmOpportunityService : ICrmOpportunityService
{
    private readonly AppDbContext _db;

    public CrmOpportunityService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<OpportunityResponse>> GetOpportunitiesAsync() =>
        (await _db.Opportunities.AsNoTracking().Include(o => o.Customer).OrderByDescending(o => o.CreatedAt).ToListAsync()).Select(Map);

    public async Task<OpportunityResponse> CreateOpportunityAsync(OpportunityRequest request)
    {
        var customer = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CustomerId)
            ?? throw new KeyNotFoundException("Cliente no encontrado");

        var opportunity = new Opportunity
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Title = request.Title,
            Description = request.Description ?? string.Empty,
            Stage = request.Stage,
            Amount = request.Amount,
            Probability = request.Probability,
            ExpectedCloseDate = request.ExpectedCloseDate
        };

        _db.Opportunities.Add(opportunity);
        await _db.SaveChangesAsync();
        opportunity.Customer = customer;
        return Map(opportunity);
    }

    public async Task<OpportunityResponse?> UpdateStageAsync(Guid id, Opportunity.OpportunityStage stage)
    {
        var opportunity = await _db.Opportunities.Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
        if (opportunity is null) return null;

        opportunity.Stage = stage;
        opportunity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(opportunity);
    }

    private static OpportunityResponse Map(Opportunity o) => new(o.Id, o.CustomerId, o.Customer?.Name ?? string.Empty, o.Title, o.Description, o.Stage, o.Amount, o.Probability, o.ExpectedCloseDate);
}
