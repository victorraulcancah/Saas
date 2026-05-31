using Backend.Application.CRM.Models;

namespace Backend.Application.CRM.Services;

public interface ICrmCustomerService
{
    Task<IEnumerable<CustomerResponse>> GetCustomersAsync();
    Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request);
    Task<CustomerResponse?> UpdateCustomerAsync(Guid id, CustomerRequest request);
}
