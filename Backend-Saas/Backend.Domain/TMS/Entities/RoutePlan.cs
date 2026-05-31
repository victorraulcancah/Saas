namespace Backend.Domain.TMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class RoutePlan : BaseEntity, ITenantEntity
{
    public enum PlanStatus
    {
        Draft,
        Optimized,
        Approved,
        Executed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string PlanNumber { get; set; } = string.Empty;
    public DateTime PlannedDate { get; set; }
    public PlanStatus Status { get; set; } = PlanStatus.Draft;
    public int TotalStops { get; set; }
    public decimal TotalDistance { get; set; }
    public decimal EstimatedDuration { get; set; }
    public decimal EstimatedCost { get; set; }
    public string OptimizationCriteria { get; set; } = string.Empty; // "Distance", "Time", "Cost"
    public DateTime? OptimizedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}
