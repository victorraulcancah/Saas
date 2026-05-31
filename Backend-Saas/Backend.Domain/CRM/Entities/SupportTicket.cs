namespace Backend.Domain.CRM.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class SupportTicket : BaseEntity, ITenantEntity
{
    public enum TicketPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TicketStatus
    {
        Open,
        InProgress,
        Resolved,
        Closed
    }

    public Guid? TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    public string Resolution { get; set; } = string.Empty;
}
