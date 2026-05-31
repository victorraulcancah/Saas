namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.PIM;

using Backend.Domain.PIM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductContentConfiguration : IEntityTypeConfiguration<ProductContent>
{
    public void Configure(EntityTypeBuilder<ProductContent> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.HasIndex(p => p.ProductId);
        
        builder.Property(p => p.Title)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(p => p.Description)
            .HasMaxLength(2000);
        
        builder.Property(p => p.Brand)
            .HasMaxLength(200);
        
        builder.Property(p => p.AttributesJson)
            .HasColumnType("jsonb");
        
        builder.Property(p => p.SeoSlug)
            .HasMaxLength(500);
        
        builder.HasIndex(p => p.SeoSlug);
    }
}
