namespace Backend.Domain.HelpDesk.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class SlaPolicy : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public HelpDeskTicket.HelpDeskPriority Priority { get; set; }
    public int FirstResponseMinutes { get; set; }
    public int ResolutionMinutes { get; set; }
    public string BusinessHours { get; set; } = string.Empty; // JSON config
    public bool IsActive { get; set; } = true;
}
