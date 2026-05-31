using Backend.Application.HR.Models;

namespace Backend.Application.HR.Services;

public interface IHrPayrollService
{
    Task<IEnumerable<PayrollResponse>> GetPayrollsAsync(DateTime? from = null, DateTime? to = null);
    Task<PayrollResponse> CalculatePayrollAsync(PayrollRequest request);
    Task<PayrollResponse?> MarkAsPaidAsync(Guid id, DateTime? paymentDate = null);
}
