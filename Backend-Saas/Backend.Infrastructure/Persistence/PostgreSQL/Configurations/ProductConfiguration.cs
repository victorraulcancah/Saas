namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations;

using Backend.Domain.ERP.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Sku).HasMaxLength(50);
        builder.Property(p => p.Barcode).HasMaxLength(50);
        builder.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Property(p => p.CostPrice).HasColumnType("decimal(18,2)");
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
