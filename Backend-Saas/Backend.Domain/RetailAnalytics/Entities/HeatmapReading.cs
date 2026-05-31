namespace Backend.Domain.RetailAnalytics.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class HeatmapReading : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid BranchId { get; set; }
    public string Zone { get; set; } = string.Empty;
    public string ZoneType { get; set; } = string.Empty; // "Entrance", "Aisle", "Display", "Checkout"
    public DateTime ReadingTime { get; set; }
    public int VisitorCount { get; set; }
    public decimal DwellTimeSeconds { get; set; }
    public decimal Temperature { get; set; } // Intensidad de tráfico
}
