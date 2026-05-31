namespace Backend.Domain.WMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class WarehouseTask : BaseEntity, ITenantEntity
{
    public enum WarehouseTaskType
    {
        Picking,
        Packing,
        Replenishment,
        Transfer
    }

    public enum WarehouseTaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string TaskNumber { get; set; } = string.Empty;
    public WarehouseTaskType Type { get; set; }
    public WarehouseTaskStatus Status { get; set; } = WarehouseTaskStatus.Pending;
    public Guid WarehouseId { get; set; }
    public string Zone { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
    public Guid? AssignedUserId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}
