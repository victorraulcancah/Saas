namespace Backend.Domain.OMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class OrderRoute : BaseEntity, ITenantEntity
{
    public enum RouteStatus
    {
        Pending,
        Optimized,
        Assigned,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public Guid OmnichannelOrderId { get; set; }
    public virtual OmnichannelOrder OmnichannelOrder { get; set; } = null!;
    public Guid? WarehouseId { get; set; }
    public Guid? BranchId { get; set; }
    public string RoutingStrategy { get; set; } = string.Empty; // "Nearest", "LeastCost", "Fastest"
    public decimal Distance { get; set; }
    public decimal EstimatedCost { get; set; }
    public int Priority { get; set; }
    public RouteStatus Status { get; set; } = RouteStatus.Pending;
    public DateTime? AssignedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}
