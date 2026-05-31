using Backend.Application.TMS.Models;

namespace Backend.Application.TMS.Services;

public interface ITmsVehicleService
{
    Task<IEnumerable<VehicleResponse>> GetVehiclesAsync();
    Task<VehicleResponse?> GetVehicleByIdAsync(Guid id);
    Task<VehicleResponse> CreateVehicleAsync(VehicleRequest request);
    Task<VehicleResponse?> UpdateVehicleAsync(Guid id, VehicleRequest request);
    Task<bool> ToggleVehicleStatusAsync(Guid id);
    Task<bool> DeleteVehicleAsync(Guid id);
}
