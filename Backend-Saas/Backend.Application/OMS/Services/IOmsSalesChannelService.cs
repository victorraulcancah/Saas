using Backend.Application.OMS.Models;

namespace Backend.Application.OMS.Services;

public interface IOmsSalesChannelService
{
    Task<IEnumerable<SalesChannelResponse>> GetSalesChannelsAsync();
    Task<SalesChannelResponse?> GetSalesChannelByIdAsync(Guid id);
    Task<SalesChannelResponse> CreateSalesChannelAsync(SalesChannelRequest request);
    Task<SalesChannelResponse?> UpdateSalesChannelAsync(Guid id, SalesChannelRequest request);
    Task<bool> ToggleChannelStatusAsync(Guid id);
    Task<bool> DeleteSalesChannelAsync(Guid id);
}
