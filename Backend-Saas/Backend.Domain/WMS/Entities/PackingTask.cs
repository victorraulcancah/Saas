namespace Backend.Domain.WMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class PackingTask : BaseEntity, ITenantEntity
{
    public enum PackingStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string PackingNumber { get; set; } = string.Empty;
    public Guid PickingTaskId { get; set; }
    public virtual PickingTask PickingTask { get; set; } = null!;
    public Guid? AssignedUserId { get; set; }
    public PackingStatus Status { get; set; } = PackingStatus.Pending;
    public int TotalBoxes { get; set; }
    public decimal TotalWeight { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}
