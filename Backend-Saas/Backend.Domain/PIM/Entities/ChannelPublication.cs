namespace Backend.Domain.PIM.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ChannelPublication : BaseEntity, ITenantEntity
{
    public enum PublicationStatus
    {
        Draft,
        Published,
        Unpublished,
        Scheduled
    }

    public Guid? TenantId { get; set; }
    public Guid ProductId { get; set; }
    public string ChannelCode { get; set; } = string.Empty; // "web", "mobile", "marketplace"
    public PublicationStatus Status { get; set; } = PublicationStatus.Draft;
    public DateTime? PublishedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public string CustomData { get; set; } = "{}"; // JSON custom data per channel
}
