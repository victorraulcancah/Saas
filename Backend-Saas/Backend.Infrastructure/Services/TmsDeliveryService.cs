using Backend.Application.TMS.Models;
using Backend.Application.TMS.Services;
using Backend.Domain.TMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class TmsDeliveryService : ITmsDeliveryService
{
    private readonly AppDbContext _db;

    public TmsDeliveryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<DeliveryResponse>> GetDeliveriesAsync() =>
        (await _db.Deliveries.AsNoTracking().OrderByDescending(d => d.CreatedAt).ToListAsync()).Select(Map);

    public async Task<DeliveryResponse?> GetDeliveryByIdAsync(Guid id)
    {
        var delivery = await _db.Deliveries.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        return delivery is null ? null : Map(delivery);
    }

    public async Task<DeliveryResponse> CreateDeliveryAsync(DeliveryRequest request)
    {
        var deliveryNumber = $"DEL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var delivery = new Delivery
        {
            Id = Guid.NewGuid(),
            DeliveryNumber = deliveryNumber,
            OrderId = request.OrderId,
            OrderType = request.OrderType,
            Status = Delivery.DeliveryStatus.Pending,
            CustomerName = request.CustomerName,
            DeliveryAddress = request.DeliveryAddress,
            ContactPhone = request.ContactPhone,
            ScheduledDate = request.ScheduledDate
        };

        _db.Deliveries.Add(delivery);
        await _db.SaveChangesAsync();
        return Map(delivery);
    }

    public async Task<DeliveryResponse?> AssignToRouteAsync(Guid id, Guid routeId, Guid vehicleId, Guid driverId)
    {
        var delivery = await _db.Deliveries.FirstOrDefaultAsync(d => d.Id == id);
        if (delivery is null) return null;

        delivery.DeliveryRouteId = routeId;
        delivery.VehicleId = vehicleId;
        delivery.DriverId = driverId;
        delivery.Status = Delivery.DeliveryStatus.Assigned;
        delivery.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(delivery);
    }

    public async Task<DeliveryResponse?> MarkAsDeliveredAsync(Guid id)
    {
        var delivery = await _db.Deliveries.FirstOrDefaultAsync(d => d.Id == id);
        if (delivery is null) return null;

        delivery.Status = Delivery.DeliveryStatus.Delivered;
        delivery.DeliveredAt = DateTime.UtcNow;
        delivery.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(delivery);
    }

    public async Task<bool> CancelDeliveryAsync(Guid id)
    {
        var delivery = await _db.Deliveries.FirstOrDefaultAsync(d => d.Id == id);
        if (delivery is null) return false;

        delivery.Status = Delivery.DeliveryStatus.Failed;
        delivery.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static DeliveryResponse Map(Delivery d) =>
        new DeliveryResponse(d.Id, d.DeliveryNumber, d.OrderId, d.OrderType, d.DeliveryRouteId, d.VehicleId, d.DriverId, d.Status, d.CustomerName, d.DeliveryAddress, d.ScheduledDate, d.DeliveredAt);
}
