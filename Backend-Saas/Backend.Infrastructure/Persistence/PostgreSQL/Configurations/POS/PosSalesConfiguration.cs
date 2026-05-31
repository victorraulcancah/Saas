namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.POS;

using Backend.Domain.POS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PosSaleConfiguration : IEntityTypeConfiguration<PosSale>
{
    public void Configure(EntityTypeBuilder<PosSale> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.SaleNumber).HasMaxLength(50).IsRequired();
        builder.Property(s => s.CashRegisterCode).HasMaxLength(50);
        builder.Property(s => s.StoreCode).HasMaxLength(50);
        builder.Property(s => s.CustomerName).HasMaxLength(200);
        builder.Property(s => s.CustomerDocument).HasMaxLength(50);
        builder.Property(s => s.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(s => s.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.TotalAmount).HasColumnType("decimal(18,2)");
        builder.HasIndex(s => new { s.TenantId, s.SaleNumber }).IsUnique();

        builder.HasMany(s => s.Items)
            .WithOne(i => i.Sale)
            .HasForeignKey(i => i.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PosSaleItemConfiguration : IEntityTypeConfiguration<PosSaleItem>
{
    public void Configure(EntityTypeBuilder<PosSaleItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
        builder.Property(i => i.Sku).HasMaxLength(50);
        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Property(i => i.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
    }
}
