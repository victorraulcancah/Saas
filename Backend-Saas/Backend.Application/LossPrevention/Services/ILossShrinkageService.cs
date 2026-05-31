using Backend.Application.LossPrevention.Models;

namespace Backend.Application.LossPrevention.Services;

public interface ILossShrinkageService
{
    Task<IEnumerable<ShrinkageCaseResponse>> GetShrinkageCasesAsync();
    Task<ShrinkageCaseResponse?> GetShrinkageCaseByIdAsync(Guid id);
    Task<ShrinkageCaseResponse> CreateShrinkageCaseAsync(ShrinkageCaseRequest request);
    Task<ShrinkageCaseResponse?> ResolveShrinkageCaseAsync(Guid id, string resolution);
}
