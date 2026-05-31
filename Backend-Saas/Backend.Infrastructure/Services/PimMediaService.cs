using Backend.Application.PIM.Models;
using Backend.Application.PIM.Services;
using Backend.Domain.PIM.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class PimMediaService : IPimMediaService
{
    private readonly AppDbContext _db;

    public PimMediaService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductMediaResponse>> GetProductMediaAsync(Guid productId) =>
        (await _db.ProductMedias.AsNoTracking().Where(m => m.ProductId == productId).OrderBy(m => m.DisplayOrder).ToListAsync()).Select(Map);

    public async Task<ProductMediaResponse?> GetMediaByIdAsync(Guid id)
    {
        var media = await _db.ProductMedias.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        return media is null ? null : Map(media);
    }

    public async Task<ProductMediaResponse> CreateProductMediaAsync(ProductMediaRequest request)
    {
        var media = new ProductMedia
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            Type = request.Type,
            Url = request.Url,
            ThumbnailUrl = request.ThumbnailUrl ?? string.Empty,
            AltText = request.AltText ?? string.Empty,
            DisplayOrder = request.DisplayOrder,
            IsPrimary = request.IsPrimary
        };

        _db.ProductMedias.Add(media);
        await _db.SaveChangesAsync();
        return Map(media);
    }

    public async Task<ProductMediaResponse?> UpdateProductMediaAsync(Guid id, ProductMediaRequest request)
    {
        var media = await _db.ProductMedias.FirstOrDefaultAsync(m => m.Id == id);
        if (media is null) return null;

        media.Type = request.Type;
        media.Url = request.Url;
        media.ThumbnailUrl = request.ThumbnailUrl ?? string.Empty;
        media.AltText = request.AltText ?? string.Empty;
        media.DisplayOrder = request.DisplayOrder;
        media.IsPrimary = request.IsPrimary;
        media.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(media);
    }

    public async Task<bool> DeleteProductMediaAsync(Guid id)
    {
        var media = await _db.ProductMedias.FirstOrDefaultAsync(m => m.Id == id);
        if (media is null) return false;

        _db.ProductMedias.Remove(media);
        await _db.SaveChangesAsync();
        return true;
    }

    private static ProductMediaResponse Map(ProductMedia m) =>
        new ProductMediaResponse(m.Id, m.ProductId, m.Type, m.Url, m.ThumbnailUrl, m.AltText, m.DisplayOrder, m.IsPrimary);
}
