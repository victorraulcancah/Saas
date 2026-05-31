namespace Backend.Domain.SFA.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class FieldCollection : BaseEntity, ITenantEntity
{
    public enum CollectionStatus
    {
        Pending,
        Collected,
        Deposited,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string CollectionNumber { get; set; } = string.Empty;
    public Guid SalesRepId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? FieldVisitId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public DateTime CollectionDate { get; set; }
    public CollectionStatus Status { get; set; } = CollectionStatus.Pending;
    public string ReceiptUrl { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
