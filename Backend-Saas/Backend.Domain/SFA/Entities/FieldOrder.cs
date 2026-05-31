namespace Backend.Domain.SFA.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class FieldOrder : BaseEntity, ITenantEntity
{
    public enum FieldOrderStatus
    {
        Draft,
        Submitted,
        Approved,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Guid SalespersonId { get; set; }
    public DateTime VisitDate { get; set; }
    public FieldOrderStatus Status { get; set; } = FieldOrderStatus.Draft;
    public decimal TotalAmount { get; set; }
    public bool IsOfflineCapture { get; set; }
    public string Notes { get; set; } = string.Empty;
}
