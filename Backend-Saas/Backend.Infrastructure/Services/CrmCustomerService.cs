using Backend.Application.CRM.Models;
using Backend.Application.CRM.Services;
using Backend.Domain.CRM.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class CrmCustomerService : ICrmCustomerService
{
    private readonly AppDbContext _db;

    public CrmCustomerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CustomerResponse>> GetCustomersAsync() =>
        (await _db.Customers.AsNoTracking().OrderBy(c => c.Name).ToListAsync()).Select(Map);

    public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            TaxId = request.TaxId,
            Email = request.Email ?? string.Empty,
            Phone = request.Phone ?? string.Empty,
            Address = request.Address ?? string.Empty,
            Type = request.Type,
            CreditLimit = request.CreditLimit,
            Notes = request.Notes ?? string.Empty,
            IsActive = true
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return Map(customer);
    }

    public async Task<CustomerResponse?> UpdateCustomerAsync(Guid id, CustomerRequest request)
    {
        var customer = await _db.Customers.FindAsync(id);
        if (customer is null) return null;

        customer.Name = request.Name;
        customer.TaxId = request.TaxId;
        customer.Email = request.Email ?? string.Empty;
        customer.Phone = request.Phone ?? string.Empty;
        customer.Address = request.Address ?? string.Empty;
        customer.Type = request.Type;
        customer.CreditLimit = request.CreditLimit;
        customer.Notes = request.Notes ?? string.Empty;
        customer.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(customer);
    }

    private static CustomerResponse Map(Customer c) => new(c.Id, c.Name, c.TaxId, c.Email, c.Phone, c.Address, c.Type, c.CreditLimit, c.IsActive, c.Notes);
}
