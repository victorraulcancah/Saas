namespace Backend.Domain.TMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Vehicle : BaseEntity, ITenantEntity
{
    public enum VehicleType
    {
        Truck,
        Van,
        Motorcycle,
        Car
    }

    public enum VehicleStatus
    {
        Available,
        InUse,
        Maintenance,
        Inactive
    }

    public Guid? TenantId { get; set; }
    public string Plate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public VehicleType Type { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Available;
    public decimal CapacityKg { get; set; }
    public decimal CapacityM3 { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public decimal FuelConsumption { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public bool IsActive { get; set; } = true;
}
