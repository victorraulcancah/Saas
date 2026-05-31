namespace Backend.Domain.PIM.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class ProductMedia : BaseEntity, ITenantEntity
{
    public enum MediaType
    {
        Image,
        Video,
        Document,
        Model3D
    }

    public Guid? TenantId { get; set; }
    public Guid ProductId { get; set; }
    public MediaType Type { get; set; }
    public string Url { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public long FileSize { get; set; }
}
