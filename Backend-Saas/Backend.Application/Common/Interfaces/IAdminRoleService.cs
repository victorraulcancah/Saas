namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Common;

public interface IAdminRoleService
{
    Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
    Task<ApplicationRole> CreateRoleAsync(string name, string? description, List<Guid>? permissionIds);
}
