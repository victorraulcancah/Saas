using Backend.Application.WMS.Models;

namespace Backend.Application.WMS.Services;

public interface IWmsTaskService
{
    Task<IEnumerable<WarehouseTaskResponse>> GetTasksAsync();
    Task<WarehouseTaskResponse> CreateTaskAsync(WarehouseTaskRequest request);
    Task<WarehouseTaskResponse?> UpdateStatusAsync(Guid id, WarehouseTaskStatusRequest request);
}
