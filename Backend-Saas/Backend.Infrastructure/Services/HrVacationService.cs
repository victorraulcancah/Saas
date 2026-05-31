using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Backend.Domain.HR.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HrVacationService : IHrVacationService
{
    private readonly AppDbContext _db;

    public HrVacationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<VacationResponse>> GetVacationsAsync() =>
        (await _db.Vacations.AsNoTracking().Include(v => v.Employee).OrderByDescending(v => v.CreatedAt).ToListAsync()).Select(Map);

    public async Task<IEnumerable<VacationResponse>> GetVacationsByEmployeeAsync(Guid employeeId) =>
        (await _db.Vacations.AsNoTracking().Include(v => v.Employee).Where(v => v.EmployeeId == employeeId).OrderByDescending(v => v.StartDate).ToListAsync()).Select(Map);

    public async Task<VacationResponse?> GetVacationByIdAsync(Guid id)
    {
        var vacation = await _db.Vacations.AsNoTracking().Include(v => v.Employee).FirstOrDefaultAsync(v => v.Id == id);
        return vacation is null ? null : Map(vacation);
    }

    public async Task<VacationResponse> CreateVacationAsync(VacationRequest request)
    {
        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == request.EmployeeId);
        if (employee is null)
            throw new InvalidOperationException("Empleado no encontrado.");

        if (request.EndDate < request.StartDate)
            throw new InvalidOperationException("La fecha de fin no puede ser anterior a la fecha de inicio.");

        var totalDays = (request.EndDate - request.StartDate).Days + 1;

        var vacation = new Vacation
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalDays = totalDays,
            Reason = request.Reason,
            Status = Vacation.VacationStatus.Pending
        };

        _db.Vacations.Add(vacation);
        await _db.SaveChangesAsync();

        vacation.Employee = employee;
        return Map(vacation);
    }

    public async Task<VacationResponse?> ApproveVacationAsync(Guid id, Guid approvedBy)
    {
        var vacation = await _db.Vacations.Include(v => v.Employee).FirstOrDefaultAsync(v => v.Id == id);
        if (vacation is null) return null;

        vacation.Status = Vacation.VacationStatus.Approved;
        vacation.ApprovedBy = approvedBy;
        vacation.ApprovedAt = DateTime.UtcNow;
        vacation.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(vacation);
    }

    public async Task<VacationResponse?> RejectVacationAsync(Guid id, string reason)
    {
        var vacation = await _db.Vacations.Include(v => v.Employee).FirstOrDefaultAsync(v => v.Id == id);
        if (vacation is null) return null;

        vacation.Status = Vacation.VacationStatus.Rejected;
        vacation.RejectionReason = reason;
        vacation.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(vacation);
    }

    public async Task<bool> CancelVacationAsync(Guid id)
    {
        var vacation = await _db.Vacations.FirstOrDefaultAsync(v => v.Id == id);
        if (vacation is null) return false;

        vacation.Status = Vacation.VacationStatus.Cancelled;
        vacation.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    private static VacationResponse Map(Vacation v)
    {
        var employeeName = $"{v.Employee.FirstName} {v.Employee.LastName}".Trim();
        return new VacationResponse(v.Id, v.EmployeeId, employeeName, v.StartDate, v.EndDate, v.TotalDays, v.Reason, v.Status, v.ApprovedBy, v.ApprovedAt, v.RejectionReason);
    }
}
