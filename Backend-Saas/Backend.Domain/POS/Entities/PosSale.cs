namespace Backend.Domain.POS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class PosSale : BaseEntity, ITenantEntity
{
    public enum SaleStatus
    {
        Draft,
        Completed,
        Cancelled
    }

    public enum PaymentMethod
    {
        Cash,
        Card,
        Yape,
        Plin,
        Mixed
    }

    public Guid? TenantId { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public string CashRegisterCode { get; set; } = string.Empty;
    public string StoreCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerDocument { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentMethod Method { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Draft;
    public ICollection<PosSaleItem> Items { get; set; } = new List<PosSaleItem>();
}
