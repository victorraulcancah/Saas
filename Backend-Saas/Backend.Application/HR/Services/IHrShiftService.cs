using Backend.Application.HR.Models;

namespace Backend.Application.HR.Services;

public interface IHrShiftService
{
    Task<IEnumerable<ShiftResponse>> GetShiftsAsync();
    Task<ShiftResponse?> GetShiftByIdAsync(Guid id);
    Task<ShiftResponse> CreateShiftAsync(ShiftRequest request);
    Task<ShiftResponse?> UpdateShiftAsync(Guid id, ShiftRequest request);
    Task<bool> DeleteShiftAsync(Guid id);
}
