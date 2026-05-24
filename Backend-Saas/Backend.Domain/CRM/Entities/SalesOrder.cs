namespace Backend.Domain.CRM.Entities;

using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;

public class SalesOrder : BaseEntity, ITenantEntity
{
    public enum SalesOrderStatus
    {
        Draft,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public SalesOrderStatus Status { get; set; }
    public virtual ICollection<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
    public decimal TotalAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
}
