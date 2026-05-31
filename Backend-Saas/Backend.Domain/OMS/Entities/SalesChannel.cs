namespace Backend.Domain.OMS.Entities;

using Backend.SharedKernel.Common;
using Backend.SharedKernel.Common.Interfaces;

public class SalesChannel : BaseEntity, ITenantEntity
{
    public enum ChannelType
    {
        Web,
        Mobile,
        Marketplace,
        WhatsApp,
        Phone,
        InStore
    }

    public Guid? TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public ChannelType Type { get; set; }
    public string ApiEndpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string Configuration { get; set; } = string.Empty; // JSON config
}
