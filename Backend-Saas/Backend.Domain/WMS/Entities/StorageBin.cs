namespace Backend.Domain.WMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class StorageBin : BaseEntity, ITenantEntity
{
    public enum BinStatus
    {
        Available,
        Occupied,
        Reserved,
        Blocked
    }

    public Guid? TenantId { get; set; }
    public Guid WarehouseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Zone { get; set; } = string.Empty;
    public string Aisle { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public BinStatus Status { get; set; } = BinStatus.Available;
    public decimal Capacity { get; set; }
    public decimal CurrentOccupancy { get; set; }
    public bool IsActive { get; set; } = true;
}
