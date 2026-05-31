using Backend.Domain.PIM.Entities;

namespace Backend.Application.PIM.Models;

// Product Content
public record ProductContentRequest(Guid ProductId, string Title, string? Description, string? Brand, string? AttributesJson, string? SeoSlug, bool IsPublished);
public record ProductContentResponse(Guid Id, Guid ProductId, string Title, string Description, string Brand, string AttributesJson, string SeoSlug, bool IsPublished);

// Product Media
public record ProductMediaRequest(Guid ProductId, ProductMedia.MediaType Type, string Url, string? ThumbnailUrl, string? AltText, int DisplayOrder, bool IsPrimary);
public record ProductMediaResponse(Guid Id, Guid ProductId, ProductMedia.MediaType Type, string Url, string ThumbnailUrl, string AltText, int DisplayOrder, bool IsPrimary);

// Product Attribute Sets
public record ProductAttributeSetRequest(string Name, string Description, Guid? CategoryId, string AttributesSchema);
public record ProductAttributeSetResponse(Guid Id, string Name, string Description, Guid? CategoryId, bool IsActive);

// Channel Publications
public record ChannelPublicationRequest(Guid ProductId, string ChannelCode, DateTime? ScheduledAt, string? CustomData);
public record ChannelPublicationResponse(Guid Id, Guid ProductId, string ChannelCode, ChannelPublication.PublicationStatus Status, DateTime? PublishedAt, DateTime? ScheduledAt);
