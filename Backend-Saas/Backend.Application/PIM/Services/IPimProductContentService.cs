using Backend.Application.PIM.Models;

namespace Backend.Application.PIM.Services;

public interface IPimProductContentService
{
    Task<IEnumerable<ProductContentResponse>> GetContentsAsync();
    Task<ProductContentResponse> CreateContentAsync(ProductContentRequest request);
    Task<ProductContentResponse?> UpdateContentAsync(Guid id, ProductContentRequest request);
}
