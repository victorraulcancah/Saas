using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Backend.Domain.HR.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HrCommissionService : IHrCommissionService
{
    private readonly AppDbContext _db;

    public HrCommissionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CommissionResponse>> GetCommissionsAsync() =>
        (await _db.Commissions.AsNoTracking().Include(c => c.Employee).OrderByDescending(c => c.CreatedAt).ToListAsync()).Select(Map);

    public async Task<IEnumerable<CommissionResponse>> GetCommissionsByEmployeeAsync(Guid employeeId) =>
        (await _db.Commissions.AsNoTracking().Include(c => c.Employee).Where(c => c.EmployeeId == employeeId).OrderByDescending(c => c.CreatedAt).ToListAsync()).Select(Map);

    public async Task<CommissionResponse?> GetCommissionByIdAsync(Guid id)
    {
        var commission = await _db.Commissions.AsNoTracking().Include(c => c.Employee).FirstOrDefaultAsync(c => c.Id == id);
        return commission is null ? null : Map(commission);
    }

    public async Task<CommissionResponse> CreateCommissionAsync(CommissionRequest request)
    {
        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == request.EmployeeId);
        if (employee is null)
            throw new InvalidOperationException("Empleado no encontrado.");

        var commissionAmount = request.BaseAmount * (request.CommissionRate / 100);

        var commission = new Commission
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            SourceType = request.SourceType,
            SourceId = request.SourceId,
            BaseAmount = request.BaseAmount,
            CommissionRate = request.CommissionRate,
            CommissionAmount = commissionAmount,
            PeriodStart = request.PeriodStart,
            PeriodEnd = request.PeriodEnd,
            Status = Commission.CommissionStatus.Calculated,
            Notes = request.Notes ?? string.Empty
        };

        _db.Commissions.Add(commission);
        await _db.SaveChangesAsync();

        commission.Employee = employee;
        return Map(commission);
    }

    public async Task<CommissionResponse?> MarkAsPaidAsync(Guid id)
    {
        var commission = await _db.Commissions.Include(c => c.Employee).FirstOrDefaultAsync(c => c.Id == id);
        if (commission is null) return null;

        commission.Status = Commission.CommissionStatus.Paid;
        commission.PaidAt = DateTime.UtcNow;
        commission.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(commission);
    }

    public async Task<bool> DeleteCommissionAsync(Guid id)
    {
        var commission = await _db.Commissions.FirstOrDefaultAsync(c => c.Id == id);
        if (commission is null) return false;

        _db.Commissions.Remove(commission);
        await _db.SaveChangesAsync();
        return true;
    }

    private static CommissionResponse Map(Commission c)
    {
        var employeeName = $"{c.Employee.FirstName} {c.Employee.LastName}".Trim();
        return new CommissionResponse(c.Id, c.EmployeeId, employeeName, c.SourceType, c.SourceId, c.BaseAmount, c.CommissionRate, c.CommissionAmount, c.PeriodStart, c.PeriodEnd, c.Status, c.PaidAt, c.Notes);
    }
}
