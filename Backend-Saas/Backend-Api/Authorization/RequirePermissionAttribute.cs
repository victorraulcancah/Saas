using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend_Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequirePermissionAttribute : TypeFilterAttribute
    {
        public RequirePermissionAttribute(string permissionKey)
            : base(typeof(RequirePermissionFilter))
        {
            Arguments = [permissionKey];
        }
    }

    public sealed class RequirePermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permissionKey;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserPermissionService _permissionService;

        public RequirePermissionFilter(
            string permissionKey,
            ICurrentUserService currentUser,
            IUserPermissionService permissionService)
        {
            _permissionKey = permissionKey;
            _currentUser = currentUser;
            _permissionService = permissionService;
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

            if (_currentUser.UserId is not { } userId)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (await _permissionService.HasPermissionAsync(userId, _permissionKey))
                return;

            context.Result = new ObjectResult(new
            {
                message = "El usuario no tiene permiso para realizar esta acción.",
                permission = _permissionKey
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
