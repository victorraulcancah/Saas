namespace Backend.Domain.PIM.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ProductAttributeSet : BaseEntity, ITenantEntity
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
    public string AttributesSchema { get; set; } = "{}"; // JSON schema
    public bool IsActive { get; set; } = true;
}
