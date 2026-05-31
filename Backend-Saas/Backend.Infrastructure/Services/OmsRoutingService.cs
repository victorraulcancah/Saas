using Backend.Application.OMS.Models;
using Backend.Application.OMS.Services;
using Backend.Domain.OMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class OmsRoutingService : IOmsRoutingService
{
    private readonly AppDbContext _db;

    public OmsRoutingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<OrderRouteResponse>> GetOrderRoutesAsync() =>
        (await _db.OrderRoutes.AsNoTracking().Include(r => r.OmnichannelOrder).OrderByDescending(r => r.CreatedAt).ToListAsync()).Select(Map);

    public async Task<IEnumerable<OrderRouteResponse>> GetRoutesByOrderAsync(Guid orderId) =>
        (await _db.OrderRoutes.AsNoTracking().Include(r => r.OmnichannelOrder).Where(r => r.OmnichannelOrderId == orderId).ToListAsync()).Select(Map);

    public async Task<OrderRouteResponse?> GetOrderRouteByIdAsync(Guid id)
    {
        var route = await _db.OrderRoutes.AsNoTracking().Include(r => r.OmnichannelOrder).FirstOrDefaultAsync(r => r.Id == id);
        return route is null ? null : Map(route);
    }

    public async Task<OrderRouteResponse> CreateOrderRouteAsync(OrderRouteRequest request)
    {
        var order = await _db.OmnichannelOrders.FirstOrDefaultAsync(o => o.Id == request.OmnichannelOrderId);
        if (order is null)
            throw new InvalidOperationException("Pedido no encontrado.");

        var route = new OrderRoute
        {
            Id = Guid.NewGuid(),
            OmnichannelOrderId = request.OmnichannelOrderId,
            WarehouseId = request.WarehouseId,
            BranchId = request.BranchId,
            RoutingStrategy = request.RoutingStrategy,
            Priority = request.Priority,
            Status = OrderRoute.RouteStatus.Pending,
            Distance = 0,
            EstimatedCost = 0
        };

        _db.OrderRoutes.Add(route);
        await _db.SaveChangesAsync();

        route.OmnichannelOrder = order;
        return Map(route);
    }

    public async Task<OrderRouteResponse?> OptimizeRouteAsync(Guid id)
    {
        var route = await _db.OrderRoutes.Include(r => r.OmnichannelOrder).FirstOrDefaultAsync(r => r.Id == id);
        if (route is null) return null;

        // Lógica de optimización de ruta (simplificada)
        route.Status = OrderRoute.RouteStatus.Optimized;
        route.Distance = new Random().Next(5, 50); // Simulación
        route.EstimatedCost = route.Distance * 2.5m; // Simulación
        route.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(route);
    }

    public async Task<OrderRouteResponse?> AssignRouteAsync(Guid id)
    {
        var route = await _db.OrderRoutes.Include(r => r.OmnichannelOrder).FirstOrDefaultAsync(r => r.Id == id);
        if (route is null) return null;

        route.Status = OrderRoute.RouteStatus.Assigned;
        route.AssignedAt = DateTime.UtcNow;
        route.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(route);
    }

    public async Task<bool> CancelRouteAsync(Guid id)
    {
        var route = await _db.OrderRoutes.FirstOrDefaultAsync(r => r.Id == id);
        if (route is null) return false;

        route.Status = OrderRoute.RouteStatus.Cancelled;
        route.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static OrderRouteResponse Map(OrderRoute r) =>
        new OrderRouteResponse(r.Id, r.OmnichannelOrderId, r.OmnichannelOrder.OrderNumber, r.WarehouseId, r.BranchId, r.RoutingStrategy, r.Distance, r.EstimatedCost, r.Priority, r.Status, r.AssignedAt);
}
