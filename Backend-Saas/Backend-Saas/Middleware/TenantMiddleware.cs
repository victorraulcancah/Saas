namespace Backend_Saas.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantIdClaim = context.User.FindFirst("TenantId")?.Value;
            if (tenantIdClaim is not null)
            {
                context.Items["TenantId"] = Guid.Parse(tenantIdClaim);
            }
        }

        await _next(context);
    }
}
