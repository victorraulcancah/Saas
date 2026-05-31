namespace Backend.Domain.WMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ReplenishmentOrder : BaseEntity, ITenantEntity
{
    public enum ReplenishmentStatus
    {
        Pending,
        Approved,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string ReplenishmentNumber { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public Guid? BranchId { get; set; }
    public ReplenishmentStatus Status { get; set; } = ReplenishmentStatus.Pending;
    public DateTime RequestedDate { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Notes { get; set; } = string.Empty;

    public virtual ICollection<ReplenishmentOrderItem> Items { get; set; } = new List<ReplenishmentOrderItem>();
}
