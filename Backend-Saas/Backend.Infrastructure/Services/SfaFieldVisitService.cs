using Backend.Application.SFA.Models;
using Backend.Application.SFA.Services;
using Backend.Domain.SFA.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class SfaFieldVisitService : ISfaFieldVisitService
{
    private readonly AppDbContext _db;

    public SfaFieldVisitService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<FieldVisitResponse>> GetFieldVisitsAsync() =>
        (await _db.FieldVisits.AsNoTracking().OrderByDescending(v => v.ScheduledDate).ToListAsync()).Select(Map);

    public async Task<FieldVisitResponse?> GetFieldVisitByIdAsync(Guid id)
    {
        var visit = await _db.FieldVisits.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        return visit is null ? null : Map(visit);
    }

    public async Task<FieldVisitResponse> CreateFieldVisitAsync(FieldVisitRequest request)
    {
        var visitNumber = $"VISIT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var visit = new FieldVisit
        {
            Id = Guid.NewGuid(),
            VisitNumber = visitNumber,
            SalesRepId = request.SalesRepId,
            CustomerId = request.CustomerId,
            ScheduledDate = request.ScheduledDate,
            Status = FieldVisit.VisitStatus.Scheduled,
            Purpose = request.Purpose,
            Notes = request.Notes ?? string.Empty
        };

        _db.FieldVisits.Add(visit);
        await _db.SaveChangesAsync();
        return Map(visit);
    }

    public async Task<FieldVisitResponse?> CheckInAsync(Guid id, string latitude, string longitude)
    {
        var visit = await _db.FieldVisits.FirstOrDefaultAsync(v => v.Id == id);
        if (visit is null) return null;

        visit.CheckInTime = DateTime.UtcNow;
        visit.CheckInLatitude = latitude;
        visit.CheckInLongitude = longitude;
        visit.Status = FieldVisit.VisitStatus.InProgress;
        visit.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(visit);
    }

    public async Task<FieldVisitResponse?> CheckOutAsync(Guid id, string latitude, string longitude)
    {
        var visit = await _db.FieldVisits.FirstOrDefaultAsync(v => v.Id == id);
        if (visit is null) return null;

        visit.CheckOutTime = DateTime.UtcNow;
        visit.CheckOutLatitude = latitude;
        visit.CheckOutLongitude = longitude;
        visit.Status = FieldVisit.VisitStatus.Completed;
        visit.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(visit);
    }

    public async Task<bool> CancelFieldVisitAsync(Guid id)
    {
        var visit = await _db.FieldVisits.FirstOrDefaultAsync(v => v.Id == id);
        if (visit is null) return false;

        visit.Status = FieldVisit.VisitStatus.Cancelled;
        visit.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static FieldVisitResponse Map(FieldVisit v) =>
        new FieldVisitResponse(v.Id, v.VisitNumber, v.SalesRepId, v.CustomerId, v.ScheduledDate, v.CheckInTime, v.CheckOutTime, v.Status, v.Purpose, v.Notes);
}
