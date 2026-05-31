using Backend.Application.OMS.Models;

namespace Backend.Application.OMS.Services;

public interface IOmsRoutingService
{
    Task<IEnumerable<OrderRouteResponse>> GetOrderRoutesAsync();
    Task<IEnumerable<OrderRouteResponse>> GetRoutesByOrderAsync(Guid orderId);
    Task<OrderRouteResponse?> GetOrderRouteByIdAsync(Guid id);
    Task<OrderRouteResponse> CreateOrderRouteAsync(OrderRouteRequest request);
    Task<OrderRouteResponse?> OptimizeRouteAsync(Guid id);
    Task<OrderRouteResponse?> AssignRouteAsync(Guid id);
    Task<bool> CancelRouteAsync(Guid id);
}
