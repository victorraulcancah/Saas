using Backend.Application.TMS.Models;

namespace Backend.Application.TMS.Services;

public interface ITmsRouteService
{
    Task<IEnumerable<DeliveryRouteResponse>> GetRoutesAsync();
    Task<DeliveryRouteResponse> CreateRouteAsync(DeliveryRouteRequest request);
    Task<DeliveryRouteResponse?> UpdateStatusAsync(Guid id, DeliveryRouteStatusRequest request);
}
