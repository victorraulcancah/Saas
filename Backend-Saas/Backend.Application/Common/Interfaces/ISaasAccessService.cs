namespace Backend.Application.Common.Interfaces;

public record SaasAccessResult(bool IsAllowed, string? Reason = null);

public interface ISaasAccessService
{
    Task<SaasAccessResult> CanAccessAsync(Guid tenantId, string? systemKey, string? moduleKey = null, string? subModuleKey = null);
}
