using Backend.Application.WMS.Models;
using Backend.Application.WMS.Services;
using Backend.Domain.WMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class WmsPickingService : IWmsPickingService
{
    private readonly AppDbContext _db;

    public WmsPickingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PickingTaskResponse>> GetPickingTasksAsync() =>
        (await _db.PickingTasks.AsNoTracking().Include(p => p.Items).OrderByDescending(p => p.CreatedAt).ToListAsync()).Select(Map);

    public async Task<PickingTaskResponse?> GetPickingTaskByIdAsync(Guid id)
    {
        var task = await _db.PickingTasks.AsNoTracking().Include(p => p.Items).FirstOrDefaultAsync(p => p.Id == id);
        return task is null ? null : Map(task);
    }

    public async Task<PickingTaskResponse> CreatePickingTaskAsync(PickingTaskRequest request)
    {
        var pickingNumber = $"PICK-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var task = new PickingTask
        {
            Id = Guid.NewGuid(),
            PickingNumber = pickingNumber,
            OrderId = request.OrderId,
            OrderType = request.OrderType,
            WarehouseId = request.WarehouseId,
            Priority = request.Priority,
            Status = PickingTask.PickingStatus.Pending
        };

        foreach (var item in request.Items)
        {
            task.Items.Add(new PickingTaskItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                LocationCode = item.LocationCode,
                QuantityRequested = item.QuantityRequested,
                QuantityPicked = 0,
                IsCompleted = false
            });
        }

        _db.PickingTasks.Add(task);
        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<PickingTaskResponse?> AssignPickingTaskAsync(Guid id, Guid userId)
    {
        var task = await _db.PickingTasks.Include(p => p.Items).FirstOrDefaultAsync(p => p.Id == id);
        if (task is null) return null;

        task.AssignedUserId = userId;
        task.Status = PickingTask.PickingStatus.Assigned;
        task.AssignedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<PickingTaskResponse?> StartPickingTaskAsync(Guid id)
    {
        var task = await _db.PickingTasks.Include(p => p.Items).FirstOrDefaultAsync(p => p.Id == id);
        if (task is null) return null;

        task.Status = PickingTask.PickingStatus.InProgress;
        task.StartedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<PickingTaskResponse?> CompletePickingTaskAsync(Guid id)
    {
        var task = await _db.PickingTasks.Include(p => p.Items).FirstOrDefaultAsync(p => p.Id == id);
        if (task is null) return null;

        task.Status = PickingTask.PickingStatus.Completed;
        task.CompletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<bool> CancelPickingTaskAsync(Guid id)
    {
        var task = await _db.PickingTasks.FirstOrDefaultAsync(p => p.Id == id);
        if (task is null) return false;

        task.Status = PickingTask.PickingStatus.Cancelled;
        task.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static PickingTaskResponse Map(PickingTask p)
    {
        var items = p.Items.Select(i => new PickingTaskItemResponse(i.Id, i.ProductId, i.ProductName, i.LocationCode, i.QuantityRequested, i.QuantityPicked, i.IsCompleted)).ToList();
        return new PickingTaskResponse(p.Id, p.PickingNumber, p.OrderId, p.OrderType, p.WarehouseId, p.AssignedUserId, p.Status, p.Priority, p.AssignedAt, p.StartedAt, p.CompletedAt, items);
    }
}
