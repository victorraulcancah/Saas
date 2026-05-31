using Backend.Application.PIM.Models;

namespace Backend.Application.PIM.Services;

public interface IPimMediaService
{
    Task<IEnumerable<ProductMediaResponse>> GetProductMediaAsync(Guid productId);
    Task<ProductMediaResponse?> GetMediaByIdAsync(Guid id);
    Task<ProductMediaResponse> CreateProductMediaAsync(ProductMediaRequest request);
    Task<ProductMediaResponse?> UpdateProductMediaAsync(Guid id, ProductMediaRequest request);
    Task<bool> DeleteProductMediaAsync(Guid id);
}
