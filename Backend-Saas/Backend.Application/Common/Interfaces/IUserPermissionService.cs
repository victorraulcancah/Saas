namespace Backend.Application.Common.Interfaces;

public interface IUserPermissionService
{
    Task<bool> HasPermissionAsync(Guid userId, string permissionKey);
}
