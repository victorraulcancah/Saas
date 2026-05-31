using Backend.Application.POS.Models;

namespace Backend.Application.POS.Services;

public interface IPosSalesService
{
    Task<IEnumerable<PosSaleResponse>> GetSalesAsync();
    Task<PosSaleResponse> CreateSaleAsync(PosSaleRequest request);
    Task<PosSaleResponse?> CompleteSaleAsync(Guid id);
    Task<PosSaleResponse?> CancelSaleAsync(Guid id);
}
