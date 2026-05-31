using Backend.Application.BI.Models;
using Backend.Application.BI.Services;
using Backend.Application.Common.Interfaces;
using Backend.Domain.BI.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class BiDashboardService : IBiDashboardService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public BiDashboardService(AppDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<DashboardResponse>> GetDashboardsAsync() =>
        (await _db.Dashboards.AsNoTracking().Where(d => d.IsActive).OrderBy(d => d.Name).ToListAsync()).Select(Map);

    public async Task<DashboardResponse?> GetDashboardByIdAsync(Guid id)
    {
        var dashboard = await _db.Dashboards.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        return dashboard is null ? null : Map(dashboard);
    }

    public async Task<DashboardResponse> CreateDashboardAsync(DashboardRequest request)
    {
        var dashboard = new Dashboard
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Layout = request.Layout,
            IsDefault = request.IsDefault,
            IsPublic = request.IsPublic,
            CreatedBy = _currentUser.UserId ?? Guid.Empty,
            RefreshIntervalMinutes = request.RefreshIntervalMinutes,
            IsActive = true
        };

        _db.Dashboards.Add(dashboard);
        await _db.SaveChangesAsync();
        return Map(dashboard);
    }

    public async Task<DashboardResponse?> UpdateDashboardAsync(Guid id, DashboardRequest request)
    {
        var dashboard = await _db.Dashboards.FirstOrDefaultAsync(d => d.Id == id);
        if (dashboard is null) return null;

        dashboard.Name = request.Name;
        dashboard.Description = request.Description;
        dashboard.Layout = request.Layout;
        dashboard.IsDefault = request.IsDefault;
        dashboard.IsPublic = request.IsPublic;
        dashboard.RefreshIntervalMinutes = request.RefreshIntervalMinutes;
        dashboard.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(dashboard);
    }

    public async Task<bool> DeleteDashboardAsync(Guid id)
    {
        var dashboard = await _db.Dashboards.FirstOrDefaultAsync(d => d.Id == id);
        if (dashboard is null) return false;

        dashboard.IsActive = false;
        dashboard.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static DashboardResponse Map(Dashboard d) =>
        new DashboardResponse(d.Id, d.Name, d.Description, d.IsDefault, d.IsPublic, d.RefreshIntervalMinutes, d.IsActive);
}
