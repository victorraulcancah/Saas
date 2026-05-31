using Backend.Application.WMS.Models;
using Backend.Application.WMS.Services;
using Backend.Domain.WMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class WmsReplenishmentService : IWmsReplenishmentService
{
    private readonly AppDbContext _db;

    public WmsReplenishmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ReplenishmentOrderResponse>> GetReplenishmentOrdersAsync() =>
        (await _db.ReplenishmentOrders.AsNoTracking().Include(r => r.Items).OrderByDescending(r => r.CreatedAt).ToListAsync()).Select(Map);

    public async Task<ReplenishmentOrderResponse?> GetReplenishmentOrderByIdAsync(Guid id)
    {
        var order = await _db.ReplenishmentOrders.AsNoTracking().Include(r => r.Items).FirstOrDefaultAsync(r => r.Id == id);
        return order is null ? null : Map(order);
    }

    public async Task<ReplenishmentOrderResponse> CreateReplenishmentOrderAsync(ReplenishmentOrderRequest request)
    {
        var replenishmentNumber = $"REPL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var order = new ReplenishmentOrder
        {
            Id = Guid.NewGuid(),
            ReplenishmentNumber = replenishmentNumber,
            WarehouseId = request.WarehouseId,
            BranchId = request.BranchId,
            Status = ReplenishmentOrder.ReplenishmentStatus.Pending,
            RequestedDate = DateTime.UtcNow,
            Notes = request.Notes ?? string.Empty
        };

        foreach (var item in request.Items)
        {
            order.Items.Add(new ReplenishmentOrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                QuantityRequested = item.QuantityRequested,
                QuantityApproved = 0,
                QuantityShipped = 0
            });
        }

        _db.ReplenishmentOrders.Add(order);
        await _db.SaveChangesAsync();
        return Map(order);
    }

    public async Task<ReplenishmentOrderResponse?> ApproveReplenishmentOrderAsync(Guid id)
    {
        var order = await _db.ReplenishmentOrders.Include(r => r.Items).FirstOrDefaultAsync(r => r.Id == id);
        if (order is null) return null;

        order.Status = ReplenishmentOrder.ReplenishmentStatus.Approved;
        order.ApprovedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        foreach (var item in order.Items)
        {
            item.QuantityApproved = item.QuantityRequested;
        }

        await _db.SaveChangesAsync();
        return Map(order);
    }

    public async Task<ReplenishmentOrderResponse?> CompleteReplenishmentOrderAsync(Guid id)
    {
        var order = await _db.ReplenishmentOrders.Include(r => r.Items).FirstOrDefaultAsync(r => r.Id == id);
        if (order is null) return null;

        order.Status = ReplenishmentOrder.ReplenishmentStatus.Completed;
        order.CompletedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(order);
    }

    public async Task<bool> CancelReplenishmentOrderAsync(Guid id)
    {
        var order = await _db.ReplenishmentOrders.FirstOrDefaultAsync(r => r.Id == id);
        if (order is null) return false;

        order.Status = ReplenishmentOrder.ReplenishmentStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static ReplenishmentOrderResponse Map(ReplenishmentOrder r)
    {
        var items = r.Items.Select(i => new ReplenishmentOrderItemResponse(i.Id, i.ProductId, i.ProductName, i.QuantityRequested, i.QuantityApproved, i.QuantityShipped)).ToList();
        return new ReplenishmentOrderResponse(r.Id, r.ReplenishmentNumber, r.WarehouseId, r.BranchId, r.Status, r.RequestedDate, r.ApprovedAt, r.CompletedAt, items);
    }
}
