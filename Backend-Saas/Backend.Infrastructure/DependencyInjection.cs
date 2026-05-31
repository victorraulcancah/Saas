namespace Backend.Infrastructure;

using Backend.Application.BI.Services;
using Backend.Application.Common.Interfaces;
using Backend.Application.CRM.Services;
using Backend.Application.ERP.Services;
using Backend.Application.HelpDesk.Services;
using Backend.Application.HR.Services;
using Backend.Application.LossPrevention.Services;
using Backend.Application.OMS.Services;
using Backend.Application.PIM.Services;
using Backend.Application.POS.Services;
using Backend.Application.RetailAnalytics.Services;
using Backend.Application.SFA.Services;
using Backend.Application.TMS.Services;
using Backend.Application.WMS.Services;
using Backend.Domain.Common;
using Backend.Infrastructure.Cache.Redis;
using Backend.Infrastructure.Persistence.MongoDB;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<ITenantUserService, TenantUserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISaasCatalogService, SaasCatalogService>();
        services.AddScoped<ISaasLicenseService, SaasLicenseService>();
        services.AddScoped<ISaasAccessService, SaasAccessService>();
        services.AddScoped<ISaasSubscriptionService, SaasSubscriptionService>();
        services.AddScoped<IUserPermissionService, UserPermissionService>();
        services.AddScoped<ErpStockService>();
        services.AddScoped<IErpCatalogService, ErpCatalogService>();
        services.AddScoped<IErpInventoryService, ErpInventoryService>();
        services.AddScoped<IErpPurchasingService, ErpPurchasingService>();
        services.AddScoped<IErpBillingService, ErpBillingService>();
        services.AddScoped<IPosSalesService, PosSalesService>();
        services.AddScoped<ICrmCustomerService, CrmCustomerService>();
        services.AddScoped<ICrmOpportunityService, CrmOpportunityService>();
        services.AddScoped<ICrmSalesOrderService, CrmSalesOrderService>();
        services.AddScoped<ICrmSupportTicketService, CrmSupportTicketService>();
        services.AddScoped<IHrEmployeeService, HrEmployeeService>();
        services.AddScoped<IHrAttendanceService, HrAttendanceService>();
        services.AddScoped<IHrPayrollService, HrPayrollService>();
        services.AddScoped<IOmsOrderService, OmsOrderService>();
        services.AddScoped<IWmsTaskService, WmsTaskService>();
        services.AddScoped<ITmsRouteService, TmsRouteService>();
        services.AddScoped<IPimProductContentService, PimProductContentService>();
        services.AddScoped<ISfaFieldOrderService, SfaFieldOrderService>();
        services.AddScoped<IHelpDeskTicketService, HelpDeskTicketService>();
        services.AddScoped<IRetailAnalyticsService, RetailAnalyticsService>();
        services.AddScoped<ILossPreventionService, LossPreventionService>();
        services.AddScoped<IBiMetricsService, BiMetricsService>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<IAdminRoleService, AdminRoleService>();
        services.AddScoped<IAuditService, MongoAuditService>();
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddHttpContextAccessor();

        var redisConnectionString = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConnectionString));
        }

        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(configuration.GetConnectionString("MongoDB")));

        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(configuration["MongoDBSettings:DatabaseName"] ?? "saas_erp_logs");
        });

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
}
