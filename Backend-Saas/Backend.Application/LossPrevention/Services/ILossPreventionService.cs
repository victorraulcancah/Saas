using Backend.Application.LossPrevention.Models;

namespace Backend.Application.LossPrevention.Services;

public interface ILossPreventionService
{
    Task<IEnumerable<CycleCountResponse>> GetCycleCountsAsync();
    Task<CycleCountResponse> CreateCycleCountAsync(CycleCountRequest request);
    Task<CycleCountResponse?> CloseCycleCountAsync(Guid id, CycleCountCloseRequest request);
}
