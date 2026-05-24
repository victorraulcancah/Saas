namespace Backend.Infrastructure;

using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Infrastructure.Cache.Redis;
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
