using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Backend.Domain.HR.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HrAttendanceService : IHrAttendanceService
{
    private readonly AppDbContext _db;

    public HrAttendanceService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AttendanceResponse>> GetAttendancesAsync(DateTime? from = null, DateTime? to = null)
    {
        var query = _db.Attendances.AsNoTracking().Include(a => a.Employee).AsQueryable();

        if (from.HasValue)
            query = query.Where(a => a.Date >= from.Value.Date);

        if (to.HasValue)
            query = query.Where(a => a.Date <= to.Value.Date);

        return (await query.OrderByDescending(a => a.Date).ToListAsync()).Select(Map);
    }

    public async Task<AttendanceResponse> RegisterAttendanceAsync(AttendanceRequest request)
    {
        var employee = await _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.EmployeeId && e.IsActive)
            ?? throw new KeyNotFoundException("Empleado no encontrado o inactivo.");

        ValidateAttendance(request);

        var attendance = new Attendance
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            Date = request.Date.Date,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            IsAbsent = request.IsAbsent,
            Justification = request.Justification ?? string.Empty
        };

        _db.Attendances.Add(attendance);
        await _db.SaveChangesAsync();
        attendance.Employee = employee;
        return Map(attendance);
    }

    public async Task<AttendanceResponse?> UpdateAttendanceAsync(Guid id, AttendanceRequest request)
    {
        var attendance = await _db.Attendances.Include(a => a.Employee).FirstOrDefaultAsync(a => a.Id == id);
        if (attendance is null) return null;

        var employee = await _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.EmployeeId && e.IsActive)
            ?? throw new KeyNotFoundException("Empleado no encontrado o inactivo.");

        ValidateAttendance(request);

        attendance.EmployeeId = request.EmployeeId;
        attendance.Employee = employee;
        attendance.Date = request.Date.Date;
        attendance.CheckIn = request.CheckIn;
        attendance.CheckOut = request.CheckOut;
        attendance.IsAbsent = request.IsAbsent;
        attendance.Justification = request.Justification ?? string.Empty;
        attendance.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(attendance);
    }

    private static void ValidateAttendance(AttendanceRequest request)
    {
        if (request.IsAbsent && (request.CheckIn.HasValue || request.CheckOut.HasValue))
            throw new InvalidOperationException("Una asistencia marcada como falta no debe tener hora de entrada o salida.");

        if (request.CheckIn.HasValue && request.CheckOut.HasValue && request.CheckOut < request.CheckIn)
            throw new InvalidOperationException("La hora de salida no puede ser anterior a la hora de entrada.");
    }

    private static AttendanceResponse Map(Attendance a)
    {
        var employeeName = $"{a.Employee?.FirstName} {a.Employee?.LastName}".Trim();
        return new AttendanceResponse(a.Id, a.EmployeeId, employeeName, a.Date, a.CheckIn, a.CheckOut, a.IsAbsent, a.Justification);
    }
}
