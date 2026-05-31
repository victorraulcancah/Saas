using Backend.Application.LossPrevention.Models;
using Backend.Application.LossPrevention.Services;
using Backend.Domain.LossPrevention.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class LossPreventionService : ILossPreventionService
{
    private readonly AppDbContext _db;

    public LossPreventionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CycleCountResponse>> GetCycleCountsAsync() =>
        (await _db.CycleCounts.AsNoTracking().OrderByDescending(c => c.ScheduledDate).ToListAsync()).Select(Map);

    public async Task<CycleCountResponse> CreateCycleCountAsync(CycleCountRequest request)
    {
        var count = new CycleCount
        {
            Id = Guid.NewGuid(),
            CountNumber = request.CountNumber,
            WarehouseId = request.WarehouseId,
            Category = request.Category ?? string.Empty,
            ScheduledDate = request.ScheduledDate,
            Notes = request.Notes ?? string.Empty
        };

        _db.CycleCounts.Add(count);
        await _db.SaveChangesAsync();
        return Map(count);
    }

    public async Task<CycleCountResponse?> CloseCycleCountAsync(Guid id, CycleCountCloseRequest request)
    {
        if (request.ItemsCounted < 0 || request.DiscrepanciesFound < 0)
            throw new InvalidOperationException("Los conteos y discrepancias no pueden ser negativos.");

        var count = await _db.CycleCounts.FirstOrDefaultAsync(c => c.Id == id);
        if (count is null) return null;

        count.Status = CycleCount.CycleCountStatus.Completed;
        count.ItemsCounted = request.ItemsCounted;
        count.DiscrepanciesFound = request.DiscrepanciesFound;
        count.Notes = request.Notes ?? count.Notes;
        count.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(count);
    }

    private static CycleCountResponse Map(CycleCount count) => new(count.Id, count.CountNumber, count.WarehouseId, count.Category, count.ScheduledDate, count.Status, count.ItemsCounted, count.DiscrepanciesFound, count.Notes);
}
