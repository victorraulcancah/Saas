namespace Backend.Domain.RetailAnalytics.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ProductPerformance : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid? BranchId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int UnitsSold { get; set; }
    public decimal Revenue { get; set; }
    public decimal Margin { get; set; }
    public int StockOuts { get; set; }
    public decimal TurnoverRate { get; set; }
}
