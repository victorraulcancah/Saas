using Backend.Application.TMS.Models;
using Backend.Application.TMS.Services;
using Backend.Domain.TMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class TmsVehicleService : ITmsVehicleService
{
    private readonly AppDbContext _db;

    public TmsVehicleService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<VehicleResponse>> GetVehiclesAsync() =>
        (await _db.Vehicles.AsNoTracking().Where(v => v.IsActive).OrderBy(v => v.Plate).ToListAsync()).Select(Map);

    public async Task<VehicleResponse?> GetVehicleByIdAsync(Guid id)
    {
        var vehicle = await _db.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        return vehicle is null ? null : Map(vehicle);
    }

    public async Task<VehicleResponse> CreateVehicleAsync(VehicleRequest request)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Plate = request.Plate,
            Brand = request.Brand,
            Model = request.Model,
            Year = request.Year,
            Type = request.Type,
            Status = Vehicle.VehicleStatus.Available,
            CapacityKg = request.CapacityKg,
            CapacityM3 = request.CapacityM3,
            FuelType = request.FuelType,
            FuelConsumption = request.FuelConsumption,
            IsActive = true
        };

        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync();
        return Map(vehicle);
    }

    public async Task<VehicleResponse?> UpdateVehicleAsync(Guid id, VehicleRequest request)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle is null) return null;

        vehicle.Plate = request.Plate;
        vehicle.Brand = request.Brand;
        vehicle.Model = request.Model;
        vehicle.Year = request.Year;
        vehicle.Type = request.Type;
        vehicle.CapacityKg = request.CapacityKg;
        vehicle.CapacityM3 = request.CapacityM3;
        vehicle.FuelType = request.FuelType;
        vehicle.FuelConsumption = request.FuelConsumption;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(vehicle);
    }

    public async Task<bool> ToggleVehicleStatusAsync(Guid id)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle is null) return false;

        vehicle.IsActive = !vehicle.IsActive;
        vehicle.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteVehicleAsync(Guid id)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle is null) return false;

        vehicle.IsActive = false;
        vehicle.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static VehicleResponse Map(Vehicle v) =>
        new VehicleResponse(v.Id, v.Plate, v.Brand, v.Model, v.Year, v.Type, v.Status, v.CapacityKg, v.CapacityM3, v.IsActive);
}
