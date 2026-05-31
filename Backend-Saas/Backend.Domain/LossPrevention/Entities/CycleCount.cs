namespace Backend.Domain.LossPrevention.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class CycleCount : BaseEntity, ITenantEntity
{
    public enum CycleCountStatus
    {
        Planned,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string CountNumber { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public CycleCountStatus Status { get; set; } = CycleCountStatus.Planned;
    public int ItemsCounted { get; set; }
    public int DiscrepanciesFound { get; set; }
    public string Notes { get; set; } = string.Empty;
}
