using Backend.Application.WMS.Models;
using Backend.Application.WMS.Services;
using Backend.Domain.WMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class WmsTaskService : IWmsTaskService
{
    private readonly AppDbContext _db;

    public WmsTaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<WarehouseTaskResponse>> GetTasksAsync() =>
        (await _db.WarehouseTasks.AsNoTracking().OrderByDescending(t => t.CreatedAt).ToListAsync()).Select(Map);

    public async Task<WarehouseTaskResponse> CreateTaskAsync(WarehouseTaskRequest request)
    {
        var task = new WarehouseTask
        {
            Id = Guid.NewGuid(),
            TaskNumber = request.TaskNumber,
            Type = request.Type,
            WarehouseId = request.WarehouseId,
            Zone = request.Zone ?? string.Empty,
            LocationCode = request.LocationCode ?? string.Empty,
            AssignedUserId = request.AssignedUserId,
            Notes = request.Notes ?? string.Empty
        };

        _db.WarehouseTasks.Add(task);
        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<WarehouseTaskResponse?> UpdateStatusAsync(Guid id, WarehouseTaskStatusRequest request)
    {
        var task = await _db.WarehouseTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return null;

        task.Status = request.Status;
        task.UpdatedAt = DateTime.UtcNow;

        if (request.Status == WarehouseTask.WarehouseTaskStatus.InProgress && task.StartedAt is null)
            task.StartedAt = DateTime.UtcNow;

        if (request.Status == WarehouseTask.WarehouseTaskStatus.Completed && task.CompletedAt is null)
            task.CompletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(task);
    }

    private static WarehouseTaskResponse Map(WarehouseTask task) => new(
        task.Id,
        task.TaskNumber,
        task.Type,
        task.Status,
        task.WarehouseId,
        task.Zone,
        task.LocationCode,
        task.AssignedUserId,
        task.StartedAt,
        task.CompletedAt,
        task.Notes);
}
