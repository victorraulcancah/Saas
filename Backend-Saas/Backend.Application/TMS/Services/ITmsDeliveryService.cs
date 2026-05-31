using Backend.Application.TMS.Models;

namespace Backend.Application.TMS.Services;

public interface ITmsDeliveryService
{
    Task<IEnumerable<DeliveryResponse>> GetDeliveriesAsync();
    Task<DeliveryResponse?> GetDeliveryByIdAsync(Guid id);
    Task<DeliveryResponse> CreateDeliveryAsync(DeliveryRequest request);
    Task<DeliveryResponse?> AssignToRouteAsync(Guid id, Guid routeId, Guid vehicleId, Guid driverId);
    Task<DeliveryResponse?> MarkAsDeliveredAsync(Guid id);
    Task<bool> CancelDeliveryAsync(Guid id);
}
