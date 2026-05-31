using Backend.Application.SFA.Models;

namespace Backend.Application.SFA.Services;

public interface ISfaFieldVisitService
{
    Task<IEnumerable<FieldVisitResponse>> GetFieldVisitsAsync();
    Task<FieldVisitResponse?> GetFieldVisitByIdAsync(Guid id);
    Task<FieldVisitResponse> CreateFieldVisitAsync(FieldVisitRequest request);
    Task<FieldVisitResponse?> CheckInAsync(Guid id, string latitude, string longitude);
    Task<FieldVisitResponse?> CheckOutAsync(Guid id, string latitude, string longitude);
    Task<bool> CancelFieldVisitAsync(Guid id);
}
