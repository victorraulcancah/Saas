using Backend.Application.CRM.Models;

namespace Backend.Application.CRM.Services;

public interface ICrmSalesOrderService
{
    Task<IEnumerable<SalesOrderResponse>> GetSalesOrdersAsync();
    Task<SalesOrderResponse> CreateSalesOrderAsync(SalesOrderRequest request);
    Task<SalesOrderResponse?> ConfirmSalesOrderAsync(Guid id);
}
