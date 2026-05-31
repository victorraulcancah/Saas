namespace Backend.Domain.CRM.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class Opportunity : BaseEntity, ITenantEntity
{
    public enum OpportunityStage
    {
        Lead,
        Qualified,
        Proposal,
        Negotiation,
        ClosedWon,
        ClosedLost
    }

    public Guid? TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public OpportunityStage Stage { get; set; }
    public decimal Amount { get; set; }
    public int Probability { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
}
