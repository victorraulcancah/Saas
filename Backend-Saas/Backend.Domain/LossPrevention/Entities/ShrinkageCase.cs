namespace Backend.Domain.LossPrevention.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ShrinkageCase : BaseEntity, ITenantEntity
{
    public enum ShrinkageType
    {
        Theft,
        Damage,
        Expiration,
        Administrative,
        Unknown
    }

    public enum CaseStatus
    {
        Reported,
        UnderInvestigation,
        Resolved,
        Closed
    }

    public Guid? TenantId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public Guid? BranchId { get; set; }
    public ShrinkageType Type { get; set; }
    public CaseStatus Status { get; set; } = CaseStatus.Reported;
    public DateTime DiscoveredAt { get; set; }
    public Guid? ReportedBy { get; set; }
    public decimal EstimatedValue { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Investigation { get; set; } = string.Empty;
    public string Resolution { get; set; } = string.Empty;
    public DateTime? ResolvedAt { get; set; }
}
