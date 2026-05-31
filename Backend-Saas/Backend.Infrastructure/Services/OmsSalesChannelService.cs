using Backend.Application.OMS.Models;
using Backend.Application.OMS.Services;
using Backend.Domain.OMS.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class OmsSalesChannelService : IOmsSalesChannelService
{
    private readonly AppDbContext _db;

    public OmsSalesChannelService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<SalesChannelResponse>> GetSalesChannelsAsync() =>
        (await _db.SalesChannels.AsNoTracking().OrderBy(c => c.Name).ToListAsync()).Select(Map);

    public async Task<SalesChannelResponse?> GetSalesChannelByIdAsync(Guid id)
    {
        var channel = await _db.SalesChannels.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return channel is null ? null : Map(channel);
    }

    public async Task<SalesChannelResponse> CreateSalesChannelAsync(SalesChannelRequest request)
    {
        var channel = new SalesChannel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Code = request.Code,
            Type = request.Type,
            ApiEndpoint = request.ApiEndpoint ?? string.Empty,
            ApiKey = request.ApiKey ?? string.Empty,
            Configuration = request.Configuration ?? string.Empty,
            IsActive = true
        };

        _db.SalesChannels.Add(channel);
        await _db.SaveChangesAsync();
        return Map(channel);
    }

    public async Task<SalesChannelResponse?> UpdateSalesChannelAsync(Guid id, SalesChannelRequest request)
    {
        var channel = await _db.SalesChannels.FirstOrDefaultAsync(c => c.Id == id);
        if (channel is null) return null;

        channel.Name = request.Name;
        channel.Code = request.Code;
        channel.Type = request.Type;
        channel.ApiEndpoint = request.ApiEndpoint ?? string.Empty;
        channel.ApiKey = request.ApiKey ?? string.Empty;
        channel.Configuration = request.Configuration ?? string.Empty;
        channel.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(channel);
    }

    public async Task<bool> ToggleChannelStatusAsync(Guid id)
    {
        var channel = await _db.SalesChannels.FirstOrDefaultAsync(c => c.Id == id);
        if (channel is null) return false;

        channel.IsActive = !channel.IsActive;
        channel.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSalesChannelAsync(Guid id)
    {
        var channel = await _db.SalesChannels.FirstOrDefaultAsync(c => c.Id == id);
        if (channel is null) return false;

        _db.SalesChannels.Remove(channel);
        await _db.SaveChangesAsync();
        return true;
    }

    private static SalesChannelResponse Map(SalesChannel c) =>
        new SalesChannelResponse(c.Id, c.Name, c.Code, c.Type, c.ApiEndpoint, c.IsActive);
}
