using Backend.Application.LossPrevention.Models;
using Backend.Application.LossPrevention.Services;
using Backend.Domain.LossPrevention.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class LossShrinkageService : ILossShrinkageService
{
    private readonly AppDbContext _db;

    public LossShrinkageService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ShrinkageCaseResponse>> GetShrinkageCasesAsync() =>
        (await _db.ShrinkageCases.AsNoTracking().OrderByDescending(c => c.DiscoveredAt).ToListAsync()).Select(Map);

    public async Task<ShrinkageCaseResponse?> GetShrinkageCaseByIdAsync(Guid id)
    {
        var shrinkageCase = await _db.ShrinkageCases.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return shrinkageCase is null ? null : Map(shrinkageCase);
    }

    public async Task<ShrinkageCaseResponse> CreateShrinkageCaseAsync(ShrinkageCaseRequest request)
    {
        var caseNumber = $"SHRINK-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var shrinkageCase = new ShrinkageCase
        {
            Id = Guid.NewGuid(),
            CaseNumber = caseNumber,
            WarehouseId = request.WarehouseId,
            BranchId = request.BranchId,
            Type = request.Type,
            Status = ShrinkageCase.CaseStatus.Reported,
            DiscoveredAt = DateTime.UtcNow,
            EstimatedValue = request.EstimatedValue,
            Description = request.Description
        };

        _db.ShrinkageCases.Add(shrinkageCase);
        await _db.SaveChangesAsync();
        return Map(shrinkageCase);
    }

    public async Task<ShrinkageCaseResponse?> ResolveShrinkageCaseAsync(Guid id, string resolution)
    {
        var shrinkageCase = await _db.ShrinkageCases.FirstOrDefaultAsync(c => c.Id == id);
        if (shrinkageCase is null) return null;

        shrinkageCase.Status = ShrinkageCase.CaseStatus.Resolved;
        shrinkageCase.Resolution = resolution;
        shrinkageCase.ResolvedAt = DateTime.UtcNow;
        shrinkageCase.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(shrinkageCase);
    }

    private static ShrinkageCaseResponse Map(ShrinkageCase c) =>
        new ShrinkageCaseResponse(c.Id, c.CaseNumber, c.WarehouseId, c.Type, c.Status, c.DiscoveredAt, c.EstimatedValue, c.Description);
}
