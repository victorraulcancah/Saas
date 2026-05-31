namespace Backend.Domain.LossPrevention.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class SuspiciousTransactionAlert : BaseEntity, ITenantEntity
{
    public enum AlertType
    {
        ExcessiveVoids,
        ExcessiveReturns,
        ExcessiveDiscounts,
        OffHourTransaction,
        LargeQuantity,
        PriceOverride,
        MultipleRefunds
    }

    public enum AlertStatus
    {
        New,
        Reviewing,
        Cleared,
        Escalated
    }

    public Guid? TenantId { get; set; }
    public string AlertNumber { get; set; } = string.Empty;
    public AlertType Type { get; set; }
    public AlertStatus Status { get; set; } = AlertStatus.New;
    public Guid TransactionId { get; set; }
    public string TransactionType { get; set; } = string.Empty; // "Sale", "Return", "Adjustment"
    public Guid? UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid? BranchId { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string RiskScore { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string ReviewNotes { get; set; } = string.Empty;
    public Guid? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
}
