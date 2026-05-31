using Backend.Application.RetailAnalytics.Models;

namespace Backend.Application.RetailAnalytics.Services;

public interface IRetailConversionService
{
    Task<IEnumerable<ConversionMetricResponse>> GetConversionMetricsAsync(Guid branchId, DateTime startDate, DateTime endDate);
    Task<ConversionMetricResponse> CreateConversionMetricAsync(ConversionMetricRequest request);
}
