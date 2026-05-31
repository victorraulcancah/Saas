namespace Backend.Domain.WMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class PickingTask : BaseEntity, ITenantEntity
{
    public enum PickingStatus
    {
        Pending,
        Assigned,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string PickingNumber { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public string OrderType { get; set; } = string.Empty; // "Sale", "Transfer", "Omnichannel"
    public Guid WarehouseId { get; set; }
    public Guid? AssignedUserId { get; set; }
    public PickingStatus Status { get; set; } = PickingStatus.Pending;
    public int Priority { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Notes { get; set; } = string.Empty;

    public virtual ICollection<PickingTaskItem> Items { get; set; } = new List<PickingTaskItem>();
}
