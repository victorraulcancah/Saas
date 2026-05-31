namespace Backend.Domain.RetailAnalytics.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ConversionMetric : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid BranchId { get; set; }
    public DateTime Date { get; set; }
    public int TotalVisitors { get; set; }
    public int TotalTransactions { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal AverageTicketValue { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PeakHour { get; set; }
}
