using Backend.Application.WMS.Models;

namespace Backend.Application.WMS.Services;

public interface IWmsPackingService
{
    Task<IEnumerable<PackingTaskResponse>> GetPackingTasksAsync();
    Task<PackingTaskResponse?> GetPackingTaskByIdAsync(Guid id);
    Task<PackingTaskResponse> CreatePackingTaskAsync(PackingTaskRequest request);
    Task<PackingTaskResponse?> StartPackingTaskAsync(Guid id);
    Task<PackingTaskResponse?> CompletePackingTaskAsync(Guid id);
    Task<bool> CancelPackingTaskAsync(Guid id);
}
