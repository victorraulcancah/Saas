namespace Backend.Domain.HelpDesk.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class TicketMessage : BaseEntity, ITenantEntity
{
    public enum MessageType
    {
        CustomerReply,
        AgentReply,
        InternalNote,
        SystemNote
    }

    public Guid? TenantId { get; set; }
    public Guid HelpDeskTicketId { get; set; }
    public virtual HelpDeskTicket HelpDeskTicket { get; set; } = null!;
    public MessageType Type { get; set; }
    public Guid? UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string AttachmentUrls { get; set; } = string.Empty; // JSON array
    public bool IsInternal { get; set; }
}
