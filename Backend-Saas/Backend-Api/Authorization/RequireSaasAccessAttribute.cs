using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend_Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequireSaasAccessAttribute : TypeFilterAttribute
    {
        public RequireSaasAccessAttribute(string systemKey, string? moduleKey = null, string? subModuleKey = null)
            : base(typeof(RequireSaasAccessFilter))
        {
            Arguments = [systemKey, moduleKey ?? string.Empty, subModuleKey ?? string.Empty];
        }
    }

    public sealed class RequireSaasAccessFilter : IAsyncAuthorizationFilter
    {
        private readonly string _systemKey;
        private readonly string? _moduleKey;
        private readonly string? _subModuleKey;
        private readonly ICurrentUserService _currentUser;
        private readonly ISaasAccessService _accessService;

        public RequireSaasAccessFilter(
            string systemKey,
            string moduleKey,
            string subModuleKey,
            ICurrentUserService currentUser,
            ISaasAccessService accessService)
        {
            _systemKey = systemKey;
            _moduleKey = string.IsNullOrWhiteSpace(moduleKey) ? null : moduleKey;
            _subModuleKey = string.IsNullOrWhiteSpace(subModuleKey) ? null : subModuleKey;
            _currentUser = currentUser;
            _accessService = accessService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (_currentUser.IsSuperAdmin)
                return;

            if (_currentUser.TenantId is not { } tenantId)
            {
                context.Result = new ForbidResult();
                return;
            }

            var result = await _accessService.CanAccessAsync(tenantId, _systemKey, _moduleKey, _subModuleKey);
            if (result.IsAllowed) return;

            context.Result = new ObjectResult(new
            {
                message = "La empresa no tiene acceso habilitado para esta función.",
                reason = result.Reason,
                system = _systemKey,
                module = _moduleKey,
                subModule = _subModuleKey
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}