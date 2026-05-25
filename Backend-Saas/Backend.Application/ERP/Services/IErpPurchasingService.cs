using Backend.Application.ERP.Models;

namespace Backend.Application.ERP.Services;

public interface IErpPurchasingService
{
    Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrdersAsync();
    Task<PurchaseOrderResponse> CreatePurchaseOrderAsync(PurchaseOrderRequest request);
    Task<PurchaseOrderResponse?> ApprovePurchaseOrderAsync(Guid id);
}
