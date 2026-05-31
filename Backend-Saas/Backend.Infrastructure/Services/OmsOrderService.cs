using Backend.Application.OMS.Models;
using Backend.Application.OMS.Services;
using Backend.Domain.OMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class OmsOrderService : IOmsOrderService
{
    private readonly AppDbContext _db;

    public OmsOrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<OmnichannelOrderResponse>> GetOrdersAsync() =>
        (await IncludeOrderGraph(_db.OmnichannelOrders.AsNoTracking())
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync()).Select(Map);

    public async Task<OmnichannelOrderResponse> CreateOrderAsync(OmnichannelOrderRequest request)
    {
        if (request.Items.Count == 0)
            throw new InvalidOperationException("El pedido omnicanal debe tener al menos un item.");

        var order = new OmnichannelOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = request.OrderNumber,
            Channel = request.Channel,
            ExternalOrderNumber = request.ExternalOrderNumber ?? string.Empty,
            CustomerId = request.CustomerId,
            OrderDate = DateTime.UtcNow,
            Status = OmnichannelOrder.OmnichannelOrderStatus.Pending,
            DeliveryMethod = request.DeliveryMethod,
            DeliveryAddress = request.DeliveryAddress ?? string.Empty,
            Notes = request.Notes ?? string.Empty
        };

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

            if (item.UnitPrice < 0)
                throw new InvalidOperationException("El precio unitario no puede ser negativo.");

            var total = item.Quantity * item.UnitPrice;
            order.Items.Add(new OmnichannelOrderItem
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
        _db.OmnichannelOrders.Add(order);
        await _db.SaveChangesAsync();
        return Map(order);
    }

    public async Task<OmnichannelOrderResponse?> AssignFulfillmentAsync(Guid id, FulfillmentAssignmentRequest request)
    {
        var order = await IncludeOrderGraph(_db.OmnichannelOrders).FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return null;

        if (order.Status == OmnichannelOrder.OmnichannelOrderStatus.Cancelled)
            throw new InvalidOperationException("No se puede asignar fulfillment a un pedido cancelado.");

        if (order.Status == OmnichannelOrder.OmnichannelOrderStatus.Fulfilled)
            throw new InvalidOperationException("No se puede reasignar un pedido ya completado.");

        order.FulfillmentAssignments.Add(new FulfillmentAssignment
        {
            Id = Guid.NewGuid(),
            WarehouseId = request.WarehouseId,
            BranchId = request.BranchId,
            FulfillmentType = request.FulfillmentType,
            Status = FulfillmentAssignment.FulfillmentAssignmentStatus.Assigned,
            AssignedAt = DateTime.UtcNow,
            Notes = request.Notes ?? string.Empty
        });

        order.Status = OmnichannelOrder.OmnichannelOrderStatus.Assigned;
        order.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(order);
    }

    public async Task<OmnichannelOrderResponse?> CancelOrderAsync(Guid id)
    {
        var order = await IncludeOrderGraph(_db.OmnichannelOrders).FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return null;

        if (order.Status == OmnichannelOrder.OmnichannelOrderStatus.Fulfilled)
            throw new InvalidOperationException("No se puede cancelar un pedido ya completado.");

        order.Status = OmnichannelOrder.OmnichannelOrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        foreach (var assignment in order.FulfillmentAssignments.Where(a => a.Status != FulfillmentAssignment.FulfillmentAssignmentStatus.Dispatched))
        {
            assignment.Status = FulfillmentAssignment.FulfillmentAssignmentStatus.Cancelled;
            assignment.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return Map(order);
    }

    private static IQueryable<OmnichannelOrder> IncludeOrderGraph(IQueryable<OmnichannelOrder> query) =>
        query.Include(o => o.Items).Include(o => o.FulfillmentAssignments);

    private static OmnichannelOrderResponse Map(OmnichannelOrder order) => new(
        order.Id,
        order.OrderNumber,
        order.Channel,
        order.ExternalOrderNumber,
        order.CustomerId,
        order.OrderDate,
        order.Status,
        order.TotalAmount,
        order.DeliveryMethod,
        order.DeliveryAddress,
        order.Notes,
        order.Items.Select(i => new OmnichannelOrderItemResponse(i.Id, i.ProductId, i.ProductName, i.Quantity, i.UnitPrice, i.TotalPrice)).ToList(),
        order.FulfillmentAssignments.Select(a => new FulfillmentAssignmentResponse(a.Id, a.OmnichannelOrderId, a.WarehouseId, a.BranchId, a.FulfillmentType, a.Status, a.AssignedAt, a.Notes)).ToList());
}
