using Backend.Application.WMS.Models;

namespace Backend.Application.WMS.Services;

public interface IWmsPickingService
{
    Task<IEnumerable<PickingTaskResponse>> GetPickingTasksAsync();
    Task<PickingTaskResponse?> GetPickingTaskByIdAsync(Guid id);
    Task<PickingTaskResponse> CreatePickingTaskAsync(PickingTaskRequest request);
    Task<PickingTaskResponse?> AssignPickingTaskAsync(Guid id, Guid userId);
    Task<PickingTaskResponse?> StartPickingTaskAsync(Guid id);
    Task<PickingTaskResponse?> CompletePickingTaskAsync(Guid id);
    Task<bool> CancelPickingTaskAsync(Guid id);
}
