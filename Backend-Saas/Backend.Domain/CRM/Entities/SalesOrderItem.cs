namespace Backend.Domain.CRM.Entities;

using Backend.Domain.Common;

public class SalesOrderItem : BaseEntity
{
    public Guid SalesOrderId { get; set; }
    public virtual SalesOrder SalesOrder { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
