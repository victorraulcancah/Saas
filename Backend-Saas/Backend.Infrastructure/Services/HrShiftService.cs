using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Backend.Domain.HR.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HrShiftService : IHrShiftService
{
    private readonly AppDbContext _db;

    public HrShiftService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ShiftResponse>> GetShiftsAsync() =>
        (await _db.Shifts.AsNoTracking().Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync()).Select(Map);

    public async Task<ShiftResponse?> GetShiftByIdAsync(Guid id)
    {
        var shift = await _db.Shifts.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        return shift is null ? null : Map(shift);
    }

    public async Task<ShiftResponse> CreateShiftAsync(ShiftRequest request)
    {
        var shift = new Shift
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            WorkDays = request.WorkDays,
            IsActive = true
        };

        _db.Shifts.Add(shift);
        await _db.SaveChangesAsync();
        return Map(shift);
    }

    public async Task<ShiftResponse?> UpdateShiftAsync(Guid id, ShiftRequest request)
    {
        var shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);
        if (shift is null) return null;

        shift.Name = request.Name;
        shift.Description = request.Description;
        shift.StartTime = request.StartTime;
        shift.EndTime = request.EndTime;
        shift.WorkDays = request.WorkDays;
        shift.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(shift);
    }

    public async Task<bool> DeleteShiftAsync(Guid id)
    {
        var shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);
        if (shift is null) return false;

        shift.IsActive = false;
        shift.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    private static ShiftResponse Map(Shift s) =>
        new ShiftResponse(s.Id, s.Name, s.Description, s.StartTime, s.EndTime, s.WorkDays, s.IsActive);
}
