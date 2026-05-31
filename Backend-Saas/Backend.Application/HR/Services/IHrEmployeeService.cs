using Backend.Application.HR.Models;

namespace Backend.Application.HR.Services;

public interface IHrEmployeeService
{
    Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync();
    Task<EmployeeResponse> CreateEmployeeAsync(EmployeeRequest request);
    Task<EmployeeResponse?> UpdateEmployeeAsync(Guid id, EmployeeRequest request);
    Task<EmployeeResponse?> DeactivateEmployeeAsync(Guid id);
}
