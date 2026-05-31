using Backend.Application.SFA.Models;
using Backend.Application.SFA.Services;
using Backend.Domain.SFA.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class SfaFieldOrderService : ISfaFieldOrderService
{
    private readonly AppDbContext _db;

    public SfaFieldOrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<FieldOrderResponse>> GetOrdersAsync() =>
        (await _db.FieldOrders.AsNoTracking().OrderByDescending(o => o.VisitDate).ToListAsync()).Select(Map);

    public async Task<FieldOrderResponse> CreateOrderAsync(FieldOrderRequest request)
    {
        if (request.TotalAmount < 0)
            throw new InvalidOperationException("El total no puede ser negativo.");

        var order = new FieldOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = request.OrderNumber,
            CustomerId = request.CustomerId,
            SalespersonId = request.SalespersonId,
            VisitDate = request.VisitDate,
            TotalAmount = request.TotalAmount,
            IsOfflineCapture = request.IsOfflineCapture,
            Notes = request.Notes ?? string.Empty
        };

        _db.FieldOrders.Add(order);
        await _db.SaveChangesAsync();
        return Map(order);
    }

    public async Task<FieldOrderResponse?> UpdateStatusAsync(Guid id, FieldOrderStatusRequest request)
    {
        var order = await _db.FieldOrders.FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return null;

        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(order);
    }

    private static FieldOrderResponse Map(FieldOrder order) => new(order.Id, order.OrderNumber, order.CustomerId, order.SalespersonId, order.VisitDate, order.Status, order.TotalAmount, order.IsOfflineCapture, order.Notes);
}
