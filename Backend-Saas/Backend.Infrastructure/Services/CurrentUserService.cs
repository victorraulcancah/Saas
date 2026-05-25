namespace Backend.Infrastructure.Services;

using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return value is not null ? Guid.Parse(value) : null;
        }
    }

    public Guid? TenantId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantId");
            return value is not null && Guid.TryParse(value, out var tenantId) ? tenantId : null;
        }
    }

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

    public bool IsSuperAdmin => Role == "SuperAdmin";
}
