using Backend.Application.ERP.Models;

namespace Backend.Application.ERP.Services;

public interface IErpBillingService
{
    Task<IEnumerable<InvoiceResponse>> GetInvoicesAsync();
    Task<InvoiceResponse> CreateInvoiceAsync(InvoiceRequest request);
    Task<InvoiceResponse?> MarkInvoiceSentAsync(Guid id);
    Task<IEnumerable<DispatchGuideResponse>> GetDispatchGuidesAsync();
    Task<DispatchGuideResponse> CreateDispatchGuideAsync(DispatchGuideRequest request);
    Task<DispatchGuideResponse?> IssueDispatchGuideAsync(Guid id);
}
