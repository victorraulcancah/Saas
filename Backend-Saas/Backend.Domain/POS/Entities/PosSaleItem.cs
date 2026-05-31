namespace Backend.Domain.POS.Entities;

using Backend.SharedKernel.Common;

public class PosSaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public PosSale Sale { get; set; } = null!;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
