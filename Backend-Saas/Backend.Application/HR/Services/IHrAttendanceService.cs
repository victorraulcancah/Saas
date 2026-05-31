using Backend.Application.HR.Models;

namespace Backend.Application.HR.Services;

public interface IHrAttendanceService
{
    Task<IEnumerable<AttendanceResponse>> GetAttendancesAsync(DateTime? from = null, DateTime? to = null);
    Task<AttendanceResponse> RegisterAttendanceAsync(AttendanceRequest request);
    Task<AttendanceResponse?> UpdateAttendanceAsync(Guid id, AttendanceRequest request);
}
