using Backend.Application.WMS.Models;
using Backend.Application.WMS.Services;
using Backend.Domain.WMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class WmsPackingService : IWmsPackingService
{
    private readonly AppDbContext _db;

    public WmsPackingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PackingTaskResponse>> GetPackingTasksAsync() =>
        (await _db.PackingTasks.AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync()).Select(Map);

    public async Task<PackingTaskResponse?> GetPackingTaskByIdAsync(Guid id)
    {
        var task = await _db.PackingTasks.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return task is null ? null : Map(task);
    }

    public async Task<PackingTaskResponse> CreatePackingTaskAsync(PackingTaskRequest request)
    {
        var packingNumber = $"PACK-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var task = new PackingTask
        {
            Id = Guid.NewGuid(),
            PackingNumber = packingNumber,
            PickingTaskId = request.PickingTaskId,
            TotalBoxes = request.TotalBoxes,
            TotalWeight = request.TotalWeight,
            Status = PackingTask.PackingStatus.Pending
        };

        _db.PackingTasks.Add(task);
        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<PackingTaskResponse?> StartPackingTaskAsync(Guid id)
    {
        var task = await _db.PackingTasks.FirstOrDefaultAsync(p => p.Id == id);
        if (task is null) return null;

        task.Status = PackingTask.PackingStatus.InProgress;
        task.StartedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<PackingTaskResponse?> CompletePackingTaskAsync(Guid id)
    {
        var task = await _db.PackingTasks.FirstOrDefaultAsync(p => p.Id == id);
        if (task is null) return null;

        task.Status = PackingTask.PackingStatus.Completed;
        task.CompletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<bool> CancelPackingTaskAsync(Guid id)
    {
        var task = await _db.PackingTasks.FirstOrDefaultAsync(p => p.Id == id);
        if (task is null) return false;

        task.Status = PackingTask.PackingStatus.Cancelled;
        task.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static PackingTaskResponse Map(PackingTask p) =>
        new PackingTaskResponse(p.Id, p.PackingNumber, p.PickingTaskId, p.AssignedUserId, p.Status, p.TotalBoxes, p.TotalWeight, p.StartedAt, p.CompletedAt);
}
