namespace Backend.Application.PIM.Models;

public record ProductContentRequest(Guid ProductId, string Title, string? Description, string? Brand, string? AttributesJson, string? SeoSlug, bool IsPublished);
public record ProductContentResponse(Guid Id, Guid ProductId, string Title, string Description, string Brand, string AttributesJson, string SeoSlug, bool IsPublished);
