using Backend.Domain.HR.Entities;

namespace Backend.Application.HR.Models;

public record EmployeeRequest(
    string FirstName,
    string LastName,
    string? Email,
    string? Phone,
    string DocumentType,
    string DocumentNumber,
    string Position,
    string Department,
    DateTime HireDate,
    decimal Salary,
    string? BankAccount);

public record EmployeeResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string Phone,
    string DocumentType,
    string DocumentNumber,
    string Position,
    string Department,
    DateTime HireDate,
    decimal Salary,
    string BankAccount,
    bool IsActive);

public record AttendanceRequest(Guid EmployeeId, DateTime Date, DateTime? CheckIn, DateTime? CheckOut, bool IsAbsent, string? Justification);
public record AttendanceResponse(Guid Id, Guid EmployeeId, string EmployeeName, DateTime Date, DateTime? CheckIn, DateTime? CheckOut, bool IsAbsent, string Justification);

public record PayrollRequest(Guid EmployeeId, DateTime PeriodStart, DateTime PeriodEnd, decimal Bonuses, decimal Deductions);
public record PayrollResponse(Guid Id, Guid EmployeeId, string EmployeeName, DateTime PeriodStart, DateTime PeriodEnd, decimal BaseSalary, decimal Bonuses, decimal Deductions, decimal NetSalary, Payroll.PayrollStatus Status, DateTime? PaymentDate);
