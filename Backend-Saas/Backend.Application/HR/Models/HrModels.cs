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

public record ShiftRequest(string Name, string Description, TimeSpan StartTime, TimeSpan EndTime, int WorkDays);
public record ShiftResponse(Guid Id, string Name, string Description, TimeSpan StartTime, TimeSpan EndTime, int WorkDays, bool IsActive);

public record CommissionRequest(Guid EmployeeId, string SourceType, Guid? SourceId, decimal BaseAmount, decimal CommissionRate, DateTime PeriodStart, DateTime PeriodEnd, string? Notes);
public record CommissionResponse(Guid Id, Guid EmployeeId, string EmployeeName, string SourceType, Guid? SourceId, decimal BaseAmount, decimal CommissionRate, decimal CommissionAmount, DateTime PeriodStart, DateTime PeriodEnd, Commission.CommissionStatus Status, DateTime? PaidAt, string Notes);

public record ContractRequest(Guid EmployeeId, string ContractNumber, Contract.ContractType Type, DateTime StartDate, DateTime? EndDate, decimal Salary, string Currency, int WorkHoursPerWeek, string Terms);
public record ContractResponse(Guid Id, Guid EmployeeId, string EmployeeName, string ContractNumber, Contract.ContractType Type, Contract.ContractStatus Status, DateTime StartDate, DateTime? EndDate, decimal Salary, string Currency, int WorkHoursPerWeek, string Terms, DateTime? TerminationDate, string? TerminationReason);

public record VacationRequest(Guid EmployeeId, DateTime StartDate, DateTime EndDate, string Reason);
public record VacationResponse(Guid Id, Guid EmployeeId, string EmployeeName, DateTime StartDate, DateTime EndDate, int TotalDays, string Reason, Vacation.VacationStatus Status, Guid? ApprovedBy, DateTime? ApprovedAt, string? RejectionReason);

public record EmployeeShiftRequest(Guid EmployeeId, Guid ShiftId, DateTime StartDate, DateTime? EndDate);
public record EmployeeShiftResponse(Guid Id, Guid EmployeeId, string EmployeeName, Guid ShiftId, string ShiftName, DateTime StartDate, DateTime? EndDate, bool IsActive);
