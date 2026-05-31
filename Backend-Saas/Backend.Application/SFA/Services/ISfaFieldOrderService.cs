using Backend.Application.SFA.Models;

namespace Backend.Application.SFA.Services;

public interface ISfaFieldOrderService
{
    Task<IEnumerable<FieldOrderResponse>> GetOrdersAsync();
    Task<FieldOrderResponse> CreateOrderAsync(FieldOrderRequest request);
    Task<FieldOrderResponse?> UpdateStatusAsync(Guid id, FieldOrderStatusRequest request);
}
