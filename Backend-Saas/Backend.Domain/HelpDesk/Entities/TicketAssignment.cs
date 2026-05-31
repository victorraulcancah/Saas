namespace Backend.Domain.HelpDesk.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class TicketAssignment : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid HelpDeskTicketId { get; set; }
    public virtual HelpDeskTicket HelpDeskTicket { get; set; } = null!;
    public Guid AgentId { get; set; }
    public string AgentName { get; set; } = string.Empty;
    public Guid? QueueId { get; set; }
    public DateTime AssignedAt { get; set; }
    public DateTime? UnassignedAt { get; set; }
    public string AssignmentReason { get; set; } = string.Empty;
}
