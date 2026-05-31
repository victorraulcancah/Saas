using Backend.Application.RetailAnalytics.Models;

namespace Backend.Application.RetailAnalytics.Services;

public interface IRetailAnalyticsService
{
    Task<IEnumerable<StoreTrafficReadingResponse>> GetTrafficReadingsAsync(DateTime? from = null, DateTime? to = null);
    Task<StoreTrafficReadingResponse> RegisterTrafficReadingAsync(StoreTrafficReadingRequest request);
}
