namespace Backend.Domain.RetailAnalytics.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class CustomerBehavior : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public DateTime VisitDate { get; set; }
    public TimeSpan DwellTime { get; set; }
    public int ZonesVisited { get; set; }
    public bool MadePurchase { get; set; }
    public decimal PurchaseAmount { get; set; }
    public string EntryPoint { get; set; } = string.Empty;
    public string ExitPoint { get; set; } = string.Empty;
}
