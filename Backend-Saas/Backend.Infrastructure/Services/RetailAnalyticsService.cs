using Backend.Application.RetailAnalytics.Models;
using Backend.Application.RetailAnalytics.Services;
using Backend.Domain.RetailAnalytics.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class RetailAnalyticsService : IRetailAnalyticsService
{
    private readonly AppDbContext _db;

    public RetailAnalyticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<StoreTrafficReadingResponse>> GetTrafficReadingsAsync(DateTime? from = null, DateTime? to = null)
    {
        var query = _db.StoreTrafficReadings.AsNoTracking().AsQueryable();

        if (from.HasValue)
            query = query.Where(r => r.ReadingAt >= from.Value);

        if (to.HasValue)
            query = query.Where(r => r.ReadingAt <= to.Value);

        return (await query.OrderByDescending(r => r.ReadingAt).ToListAsync()).Select(Map);
    }

    public async Task<StoreTrafficReadingResponse> RegisterTrafficReadingAsync(StoreTrafficReadingRequest request)
    {
        if (request.VisitorsIn < 0 || request.VisitorsOut < 0 || request.TicketsCount < 0 || request.SalesAmount < 0)
            throw new InvalidOperationException("Los valores de tráfico, tickets y ventas no pueden ser negativos.");

        var reading = new StoreTrafficReading
        {
            Id = Guid.NewGuid(),
            BranchId = request.BranchId,
            ReadingAt = request.ReadingAt,
            VisitorsIn = request.VisitorsIn,
            VisitorsOut = request.VisitorsOut,
            TicketsCount = request.TicketsCount,
            SalesAmount = request.SalesAmount,
            Source = request.Source ?? string.Empty
        };

        _db.StoreTrafficReadings.Add(reading);
        await _db.SaveChangesAsync();
        return Map(reading);
    }

    private static StoreTrafficReadingResponse Map(StoreTrafficReading reading)
    {
        var conversionRate = reading.VisitorsIn == 0 ? 0 : Math.Round((decimal)reading.TicketsCount / reading.VisitorsIn * 100, 2);
        return new StoreTrafficReadingResponse(reading.Id, reading.BranchId, reading.ReadingAt, reading.VisitorsIn, reading.VisitorsOut, reading.TicketsCount, reading.SalesAmount, conversionRate, reading.Source);
    }
}
