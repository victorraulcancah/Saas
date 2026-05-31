using Backend.Application.CRM.Models;
using Backend.Application.CRM.Services;
using Backend.Domain.CRM.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class CrmSalesOrderService : ICrmSalesOrderService
{
    private readonly AppDbContext _db;

    public CrmSalesOrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<SalesOrderResponse>> GetSalesOrdersAsync() =>
        (await _db.SalesOrders.AsNoTracking().Include(o => o.Customer).Include(o => o.Items).OrderByDescending(o => o.OrderDate).ToListAsync()).Select(Map);

    public async Task<SalesOrderResponse> CreateSalesOrderAsync(SalesOrderRequest request)
    {
        var customer = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CustomerId)
            ?? throw new KeyNotFoundException("Cliente no encontrado");

        if (request.Items.Count == 0)
            throw new InvalidOperationException("La orden debe tener al menos un item.");

        var order = new SalesOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = request.OrderNumber,
            CustomerId = request.CustomerId,
            OrderDate = DateTime.UtcNow,
            Status = SalesOrder.SalesOrderStatus.Draft,
            Notes = request.Notes ?? string.Empty
        };

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

            var total = item.Quantity * item.UnitPrice;
            order.Items.Add(new SalesOrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = total
            });
        }

        order.TotalAmount = order.Items.Sum(i => i.TotalPrice);
        _db.SalesOrders.Add(order);
        await _db.SaveChangesAsync();
        order.Customer = customer;
        return Map(order);
    }

    public async Task<SalesOrderResponse?> ConfirmSalesOrderAsync(Guid id)
    {
        var order = await _db.SalesOrders.Include(o => o.Customer).Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return null;
        if (order.Status != SalesOrder.SalesOrderStatus.Draft)
            throw new InvalidOperationException("Solo se puede confirmar una orden en borrador.");

        order.Status = SalesOrder.SalesOrderStatus.Confirmed;
        order.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(order);
    }

    private static SalesOrderResponse Map(SalesOrder o) => new(
        o.Id,
        o.OrderNumber,
        o.CustomerId,
        o.Customer?.Name ?? string.Empty,
        o.OrderDate,
        o.Status,
        o.TotalAmount,
        o.Notes,
        o.Items.Select(i => new SalesOrderItemResponse(i.Id, i.ProductId, i.ProductName, i.Quantity, i.UnitPrice, i.TotalPrice)).ToList());
}
