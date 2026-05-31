namespace Backend.Domain.RetailAnalytics.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class StoreTrafficReading : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid BranchId { get; set; }
    public DateTime ReadingAt { get; set; }
    public int VisitorsIn { get; set; }
    public int VisitorsOut { get; set; }
    public int TicketsCount { get; set; }
    public decimal SalesAmount { get; set; }
    public string Source { get; set; } = string.Empty;
}
