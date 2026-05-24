namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Common;

public interface IAdminUserService
{
    Task<IEnumerable<ApplicationUser>> GetUsersAsync();
    Task<ApplicationUser> CreateUserAsync(string email, string password, string? firstName, string? lastName, List<Guid>? roleIds);
}
