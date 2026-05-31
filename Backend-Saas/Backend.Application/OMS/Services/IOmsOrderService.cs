using Backend.Application.OMS.Models;

namespace Backend.Application.OMS.Services;

public interface IOmsOrderService
{
    Task<IEnumerable<OmnichannelOrderResponse>> GetOrdersAsync();
    Task<OmnichannelOrderResponse> CreateOrderAsync(OmnichannelOrderRequest request);
    Task<OmnichannelOrderResponse?> AssignFulfillmentAsync(Guid id, FulfillmentAssignmentRequest request);
    Task<OmnichannelOrderResponse?> CancelOrderAsync(Guid id);
}
