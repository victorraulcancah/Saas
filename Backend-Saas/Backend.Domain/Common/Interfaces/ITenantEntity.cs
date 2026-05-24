namespace Backend.Domain.Common.Interfaces;

public interface ITenantEntity
{
    Guid? TenantId { get; set; }
}
