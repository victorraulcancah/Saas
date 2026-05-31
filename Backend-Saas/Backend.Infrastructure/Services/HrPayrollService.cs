using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Backend.Domain.HR.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HrPayrollService : IHrPayrollService
{
    private readonly AppDbContext _db;

    public HrPayrollService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PayrollResponse>> GetPayrollsAsync(DateTime? from = null, DateTime? to = null)
    {
        var query = _db.Payrolls.AsNoTracking().Include(p => p.Employee).AsQueryable();

        if (from.HasValue)
            query = query.Where(p => p.PeriodEnd >= from.Value.Date);

        if (to.HasValue)
            query = query.Where(p => p.PeriodStart <= to.Value.Date);

        return (await query.OrderByDescending(p => p.PeriodEnd).ToListAsync()).Select(Map);
    }

    public async Task<PayrollResponse> CalculatePayrollAsync(PayrollRequest request)
    {
        if (request.PeriodEnd < request.PeriodStart)
            throw new InvalidOperationException("La fecha final del periodo no puede ser anterior a la fecha inicial.");

        if (request.Bonuses < 0 || request.Deductions < 0)
            throw new InvalidOperationException("Bonificaciones y descuentos no pueden ser negativos.");

        var employee = await _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.EmployeeId && e.IsActive)
            ?? throw new KeyNotFoundException("Empleado no encontrado o inactivo.");

        var payroll = new Payroll
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            PeriodStart = request.PeriodStart.Date,
            PeriodEnd = request.PeriodEnd.Date,
            BaseSalary = employee.Salary,
            Bonuses = request.Bonuses,
            Deductions = request.Deductions,
            NetSalary = employee.Salary + request.Bonuses - request.Deductions,
            Status = Payroll.PayrollStatus.Calculated
        };

        _db.Payrolls.Add(payroll);
        await _db.SaveChangesAsync();
        payroll.Employee = employee;
        return Map(payroll);
    }

    public async Task<PayrollResponse?> MarkAsPaidAsync(Guid id, DateTime? paymentDate = null)
    {
        var payroll = await _db.Payrolls.Include(p => p.Employee).FirstOrDefaultAsync(p => p.Id == id);
        if (payroll is null) return null;

        if (payroll.Status == Payroll.PayrollStatus.Paid)
            return Map(payroll);

        payroll.Status = Payroll.PayrollStatus.Paid;
        payroll.PaymentDate = paymentDate ?? DateTime.UtcNow;
        payroll.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(payroll);
    }

    private static PayrollResponse Map(Payroll p)
    {
        var employeeName = $"{p.Employee?.FirstName} {p.Employee?.LastName}".Trim();
        return new PayrollResponse(p.Id, p.EmployeeId, employeeName, p.PeriodStart, p.PeriodEnd, p.BaseSalary, p.Bonuses, p.Deductions, p.NetSalary, p.Status, p.PaymentDate);
    }
}
