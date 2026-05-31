using Backend.Application.HR.Models;
using Backend.Application.HR.Services;
using Backend.Domain.HR.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class HrEmployeeService : IHrEmployeeService
{
    private readonly AppDbContext _db;

    public HrEmployeeService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync() =>
        (await _db.Employees.AsNoTracking().OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToListAsync()).Select(Map);

    public async Task<EmployeeResponse> CreateEmployeeAsync(EmployeeRequest request)
    {
        if (request.Salary < 0)
            throw new InvalidOperationException("El salario no puede ser negativo.");

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email ?? string.Empty,
            Phone = request.Phone ?? string.Empty,
            DocumentType = request.DocumentType,
            DocumentNumber = request.DocumentNumber,
            Position = request.Position,
            Department = request.Department,
            HireDate = request.HireDate,
            Salary = request.Salary,
            BankAccount = request.BankAccount ?? string.Empty,
            IsActive = true
        };

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return Map(employee);
    }

    public async Task<EmployeeResponse?> UpdateEmployeeAsync(Guid id, EmployeeRequest request)
    {
        if (request.Salary < 0)
            throw new InvalidOperationException("El salario no puede ser negativo.");

        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee is null) return null;

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email ?? string.Empty;
        employee.Phone = request.Phone ?? string.Empty;
        employee.DocumentType = request.DocumentType;
        employee.DocumentNumber = request.DocumentNumber;
        employee.Position = request.Position;
        employee.Department = request.Department;
        employee.HireDate = request.HireDate;
        employee.Salary = request.Salary;
        employee.BankAccount = request.BankAccount ?? string.Empty;
        employee.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(employee);
    }

    public async Task<EmployeeResponse?> DeactivateEmployeeAsync(Guid id)
    {
        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee is null) return null;

        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(employee);
    }

    private static EmployeeResponse Map(Employee e)
    {
        var fullName = $"{e.FirstName} {e.LastName}".Trim();
        return new EmployeeResponse(e.Id, e.FirstName, e.LastName, fullName, e.Email, e.Phone, e.DocumentType, e.DocumentNumber, e.Position, e.Department, e.HireDate, e.Salary, e.BankAccount, e.IsActive);
    }
}
