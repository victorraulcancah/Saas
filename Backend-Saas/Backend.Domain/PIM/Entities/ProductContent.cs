namespace Backend.Domain.PIM.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ProductContent : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public Guid ProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string AttributesJson { get; set; } = "{}";
    public string SeoSlug { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
}
