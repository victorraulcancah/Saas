using Backend.Application.HR.Models;

namespace Backend.Application.HR.Services;

public interface IHrContractService
{
    Task<IEnumerable<ContractResponse>> GetContractsAsync();
    Task<IEnumerable<ContractResponse>> GetContractsByEmployeeAsync(Guid employeeId);
    Task<ContractResponse?> GetContractByIdAsync(Guid id);
    Task<ContractResponse> CreateContractAsync(ContractRequest request);
    Task<ContractResponse?> UpdateContractAsync(Guid id, ContractRequest request);
    Task<ContractResponse?> TerminateContractAsync(Guid id, string reason);
    Task<bool> DeleteContractAsync(Guid id);
}
