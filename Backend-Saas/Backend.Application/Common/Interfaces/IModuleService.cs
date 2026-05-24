namespace Backend.Application.Common.Interfaces;

using Backend.Domain.Common;

public interface IModuleService
{
    Task<IEnumerable<Module>> GetAllModulesAsync();
    Task<Module> CreateModuleAsync(string name, string key, string? description, string? icon);
}
