using Backend.Application.WMS.Models;

namespace Backend.Application.WMS.Services;

public interface IWmsReplenishmentService
{
    Task<IEnumerable<ReplenishmentOrderResponse>> GetReplenishmentOrdersAsync();
    Task<ReplenishmentOrderResponse?> GetReplenishmentOrderByIdAsync(Guid id);
    Task<ReplenishmentOrderResponse> CreateReplenishmentOrderAsync(ReplenishmentOrderRequest request);
    Task<ReplenishmentOrderResponse?> ApproveReplenishmentOrderAsync(Guid id);
    Task<ReplenishmentOrderResponse?> CompleteReplenishmentOrderAsync(Guid id);
    Task<bool> CancelReplenishmentOrderAsync(Guid id);
}
