using Backend.Application.PIM.Models;
using Backend.Application.PIM.Services;
using Backend.Domain.PIM.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class PimProductContentService : IPimProductContentService
{
    private readonly AppDbContext _db;

    public PimProductContentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductContentResponse>> GetContentsAsync() =>
        (await _db.ProductContents.AsNoTracking().OrderBy(c => c.Title).ToListAsync()).Select(Map);

    public async Task<ProductContentResponse> CreateContentAsync(ProductContentRequest request)
    {
        var content = new ProductContent
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            Title = request.Title,
            Description = request.Description ?? string.Empty,
            Brand = request.Brand ?? string.Empty,
            AttributesJson = string.IsNullOrWhiteSpace(request.AttributesJson) ? "{}" : request.AttributesJson,
            SeoSlug = request.SeoSlug ?? string.Empty,
            IsPublished = request.IsPublished
        };

        _db.ProductContents.Add(content);
        await _db.SaveChangesAsync();
        return Map(content);
    }

    public async Task<ProductContentResponse?> UpdateContentAsync(Guid id, ProductContentRequest request)
    {
        var content = await _db.ProductContents.FirstOrDefaultAsync(c => c.Id == id);
        if (content is null) return null;

        content.ProductId = request.ProductId;
        content.Title = request.Title;
        content.Description = request.Description ?? string.Empty;
        content.Brand = request.Brand ?? string.Empty;
        content.AttributesJson = string.IsNullOrWhiteSpace(request.AttributesJson) ? "{}" : request.AttributesJson;
        content.SeoSlug = request.SeoSlug ?? string.Empty;
        content.IsPublished = request.IsPublished;
        content.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(content);
    }

    private static ProductContentResponse Map(ProductContent content) => new(content.Id, content.ProductId, content.Title, content.Description, content.Brand, content.AttributesJson, content.SeoSlug, content.IsPublished);
}
