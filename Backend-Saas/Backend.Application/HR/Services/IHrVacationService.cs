using Backend.Application.HR.Models;

namespace Backend.Application.HR.Services;

public interface IHrVacationService
{
    Task<IEnumerable<VacationResponse>> GetVacationsAsync();
    Task<IEnumerable<VacationResponse>> GetVacationsByEmployeeAsync(Guid employeeId);
    Task<VacationResponse?> GetVacationByIdAsync(Guid id);
    Task<VacationResponse> CreateVacationAsync(VacationRequest request);
    Task<VacationResponse?> ApproveVacationAsync(Guid id, Guid approvedBy);
    Task<VacationResponse?> RejectVacationAsync(Guid id, string reason);
    Task<bool> CancelVacationAsync(Guid id);
}
