using Backend.Application.TMS.Models;
using Backend.Application.TMS.Services;
using Backend.Domain.TMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class TmsRouteService : ITmsRouteService
{
    private readonly AppDbContext _db;

    public TmsRouteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<DeliveryRouteResponse>> GetRoutesAsync() =>
        (await _db.DeliveryRoutes.AsNoTracking().OrderByDescending(r => r.PlannedDate).ToListAsync()).Select(Map);

    public async Task<DeliveryRouteResponse> CreateRouteAsync(DeliveryRouteRequest request)
    {
        if (request.CapacityKg < 0)
            throw new InvalidOperationException("La capacidad no puede ser negativa.");

        var route = new DeliveryRoute
        {
            Id = Guid.NewGuid(),
            RouteNumber = request.RouteNumber,
            VehiclePlate = request.VehiclePlate ?? string.Empty,
            DriverName = request.DriverName ?? string.Empty,
            PlannedDate = request.PlannedDate,
            CapacityKg = request.CapacityKg,
            Notes = request.Notes ?? string.Empty
        };

        _db.DeliveryRoutes.Add(route);
        await _db.SaveChangesAsync();
        return Map(route);
    }

    public async Task<DeliveryRouteResponse?> UpdateStatusAsync(Guid id, DeliveryRouteStatusRequest request)
    {
        var route = await _db.DeliveryRoutes.FirstOrDefaultAsync(r => r.Id == id);
        if (route is null) return null;

        route.Status = request.Status;
        route.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(route);
    }

    private static DeliveryRouteResponse Map(DeliveryRoute route) => new(route.Id, route.RouteNumber, route.VehiclePlate, route.DriverName, route.PlannedDate, route.Status, route.CapacityKg, route.Notes);
}
