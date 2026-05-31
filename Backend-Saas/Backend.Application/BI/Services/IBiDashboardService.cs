using Backend.Application.BI.Models;

namespace Backend.Application.BI.Services;

public interface IBiDashboardService
{
    Task<IEnumerable<DashboardResponse>> GetDashboardsAsync();
    Task<DashboardResponse?> GetDashboardByIdAsync(Guid id);
    Task<DashboardResponse> CreateDashboardAsync(DashboardRequest request);
    Task<DashboardResponse?> UpdateDashboardAsync(Guid id, DashboardRequest request);
    Task<bool> DeleteDashboardAsync(Guid id);
}
