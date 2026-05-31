namespace Backend.Domain.SFA.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class FieldVisit : BaseEntity, ITenantEntity
{
    public enum VisitStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid? TenantId { get; set; }
    public string VisitNumber { get; set; } = string.Empty;
    public Guid SalesRepId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string CheckInLatitude { get; set; } = string.Empty;
    public string CheckInLongitude { get; set; } = string.Empty;
    public string CheckOutLatitude { get; set; } = string.Empty;
    public string CheckOutLongitude { get; set; } = string.Empty;
    public VisitStatus Status { get; set; } = VisitStatus.Scheduled;
    public string Purpose { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string PhotoUrls { get; set; } = string.Empty; // JSON array
}
