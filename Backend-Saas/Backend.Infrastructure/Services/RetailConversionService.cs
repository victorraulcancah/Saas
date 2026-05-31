using Backend.Application.RetailAnalytics.Models;
using Backend.Application.RetailAnalytics.Services;
using Backend.Domain.RetailAnalytics.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class RetailConversionService : IRetailConversionService
{
    private readonly AppDbContext _db;

    public RetailConversionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ConversionMetricResponse>> GetConversionMetricsAsync(Guid branchId, DateTime startDate, DateTime endDate) =>
        (await _db.ConversionMetrics.AsNoTracking().Where(m => m.BranchId == branchId && m.Date >= startDate && m.Date <= endDate).OrderBy(m => m.Date).ToListAsync()).Select(Map);

    public async Task<ConversionMetricResponse> CreateConversionMetricAsync(ConversionMetricRequest request)
    {
        var conversionRate = request.TotalVisitors > 0 ? (decimal)request.TotalTransactions / request.TotalVisitors * 100 : 0;
        var totalRevenue = request.TotalTransactions * request.AverageTicketValue;

        var metric = new ConversionMetric
        {
            Id = Guid.NewGuid(),
            BranchId = request.BranchId,
            Date = request.Date,
            TotalVisitors = request.TotalVisitors,
            TotalTransactions = request.TotalTransactions,
            ConversionRate = conversionRate,
            AverageTicketValue = request.AverageTicketValue,
            TotalRevenue = totalRevenue,
            PeakHour = DateTime.Now.Hour
        };

        _db.ConversionMetrics.Add(metric);
        await _db.SaveChangesAsync();
        return Map(metric);
    }

    private static ConversionMetricResponse Map(ConversionMetric m) =>
        new ConversionMetricResponse(m.Id, m.BranchId, m.Date, m.TotalVisitors, m.TotalTransactions, m.ConversionRate, m.AverageTicketValue, m.TotalRevenue);
}
