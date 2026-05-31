using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Backend.Domain.HR.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HrContractService : IHrContractService
{
    private readonly AppDbContext _db;

    public HrContractService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ContractResponse>> GetContractsAsync() =>
        (await _db.Contracts.AsNoTracking().Include(c => c.Employee).OrderByDescending(c => c.StartDate).ToListAsync()).Select(Map);

    public async Task<IEnumerable<ContractResponse>> GetContractsByEmployeeAsync(Guid employeeId) =>
        (await _db.Contracts.AsNoTracking().Include(c => c.Employee).Where(c => c.EmployeeId == employeeId).OrderByDescending(c => c.StartDate).ToListAsync()).Select(Map);

    public async Task<ContractResponse?> GetContractByIdAsync(Guid id)
    {
        var contract = await _db.Contracts.AsNoTracking().Include(c => c.Employee).FirstOrDefaultAsync(c => c.Id == id);
        return contract is null ? null : Map(contract);
    }

    public async Task<ContractResponse> CreateContractAsync(ContractRequest request)
    {
        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == request.EmployeeId);
        if (employee is null)
            throw new InvalidOperationException("Empleado no encontrado.");

        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            ContractNumber = request.ContractNumber,
            Type = request.Type,
            Status = Contract.ContractStatus.Active,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Salary = request.Salary,
            Currency = request.Currency,
            WorkHoursPerWeek = request.WorkHoursPerWeek,
            Terms = request.Terms
        };

        _db.Contracts.Add(contract);
        await _db.SaveChangesAsync();

        contract.Employee = employee;
        return Map(contract);
    }

    public async Task<ContractResponse?> UpdateContractAsync(Guid id, ContractRequest request)
    {
        var contract = await _db.Contracts.Include(c => c.Employee).FirstOrDefaultAsync(c => c.Id == id);
        if (contract is null) return null;

        contract.ContractNumber = request.ContractNumber;
        contract.Type = request.Type;
        contract.StartDate = request.StartDate;
        contract.EndDate = request.EndDate;
        contract.Salary = request.Salary;
        contract.Currency = request.Currency;
        contract.WorkHoursPerWeek = request.WorkHoursPerWeek;
        contract.Terms = request.Terms;
        contract.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(contract);
    }

    public async Task<ContractResponse?> TerminateContractAsync(Guid id, string reason)
    {
        var contract = await _db.Contracts.Include(c => c.Employee).FirstOrDefaultAsync(c => c.Id == id);
        if (contract is null) return null;

        contract.Status = Contract.ContractStatus.Terminated;
        contract.TerminationDate = DateTime.UtcNow;
        contract.TerminationReason = reason;
        contract.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(contract);
    }

    public async Task<bool> DeleteContractAsync(Guid id)
    {
        var contract = await _db.Contracts.FirstOrDefaultAsync(c => c.Id == id);
        if (contract is null) return false;

        _db.Contracts.Remove(contract);
        await _db.SaveChangesAsync();
        return true;
    }

    private static ContractResponse Map(Contract c)
    {
        var employeeName = $"{c.Employee.FirstName} {c.Employee.LastName}".Trim();
        return new ContractResponse(c.Id, c.EmployeeId, employeeName, c.ContractNumber, c.Type, c.Status, c.StartDate, c.EndDate, c.Salary, c.Currency, c.WorkHoursPerWeek, c.Terms, c.TerminationDate, c.TerminationReason);
    }
}
