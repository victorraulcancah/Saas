using Backend.Application.BI.Models;
using Backend.Application.BI.Services;
using Backend.Domain.BI.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class BiMetricsService : IBiMetricsService
{
    private readonly AppDbContext _db;

    public BiMetricsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<MetricSnapshotResponse>> GetMetricsAsync(DateTime? from = null, DateTime? to = null)
    {
        var query = _db.MetricSnapshots.AsNoTracking().AsQueryable();

        if (from.HasValue)
            query = query.Where(m => m.PeriodEnd >= from.Value.Date);

        if (to.HasValue)
            query = query.Where(m => m.PeriodStart <= to.Value.Date);

        return (await query.OrderByDescending(m => m.PeriodEnd).ToListAsync()).Select(Map);
    }

    public async Task<MetricSnapshotResponse> CreateMetricSnapshotAsync(MetricSnapshotRequest request)
    {
        if (request.PeriodEnd < request.PeriodStart)
            throw new InvalidOperationException("La fecha final del periodo no puede ser anterior a la fecha inicial.");

        var snapshot = new MetricSnapshot
        {
            Id = Guid.NewGuid(),
            MetricKey = request.MetricKey,
            MetricName = request.MetricName,
            PeriodStart = request.PeriodStart.Date,
            PeriodEnd = request.PeriodEnd.Date,
            Value = request.Value,
            Dimension = request.Dimension ?? string.Empty,
            SourceSystem = request.SourceSystem ?? string.Empty
        };

        _db.MetricSnapshots.Add(snapshot);
        await _db.SaveChangesAsync();
        return Map(snapshot);
    }

    private static MetricSnapshotResponse Map(MetricSnapshot snapshot) => new(snapshot.Id, snapshot.MetricKey, snapshot.MetricName, snapshot.PeriodStart, snapshot.PeriodEnd, snapshot.Value, snapshot.Dimension, snapshot.SourceSystem);
}
