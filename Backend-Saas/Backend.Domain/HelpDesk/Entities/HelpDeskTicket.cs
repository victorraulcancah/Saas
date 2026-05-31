namespace Backend.Domain.HelpDesk.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class HelpDeskTicket : BaseEntity, ITenantEntity
{
    public enum HelpDeskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum HelpDeskStatus
    {
        Open,
        Assigned,
        InProgress,
        Resolved,
        Closed
    }

    public Guid? TenantId { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public HelpDeskPriority Priority { get; set; } = HelpDeskPriority.Medium;
    public HelpDeskStatus Status { get; set; } = HelpDeskStatus.Open;
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime? FirstResponseDueAt { get; set; }
    public DateTime? ResolutionDueAt { get; set; }
    public string Resolution { get; set; } = string.Empty;
}
