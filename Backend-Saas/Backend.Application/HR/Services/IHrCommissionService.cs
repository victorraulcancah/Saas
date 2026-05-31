using Backend.Application.HR.Models;

namespace Backend.Application.HR.Services;

public interface IHrCommissionService
{
    Task<IEnumerable<CommissionResponse>> GetCommissionsAsync();
    Task<IEnumerable<CommissionResponse>> GetCommissionsByEmployeeAsync(Guid employeeId);
    Task<CommissionResponse?> GetCommissionByIdAsync(Guid id);
    Task<CommissionResponse> CreateCommissionAsync(CommissionRequest request);
    Task<CommissionResponse?> MarkAsPaidAsync(Guid id);
    Task<bool> DeleteCommissionAsync(Guid id);
}
