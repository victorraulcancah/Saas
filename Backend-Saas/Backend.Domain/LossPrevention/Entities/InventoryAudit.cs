namespace Backend.Domain.LossPrevention.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class InventoryAudit : BaseEntity, ITenantEntity
{
    public enum AuditStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string AuditNumber { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public string AuditType { get; set; } = string.Empty; // "Full", "Partial", "Spot"
    public AuditStatus Status { get; set; } = AuditStatus.Scheduled;
    public DateTime ScheduledDate { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? AuditorId { get; set; }
    public int TotalItemsAudited { get; set; }
    public int DiscrepanciesFound { get; set; }
    public decimal TotalVarianceValue { get; set; }
    public string Notes { get; set; } = string.Empty;
}
