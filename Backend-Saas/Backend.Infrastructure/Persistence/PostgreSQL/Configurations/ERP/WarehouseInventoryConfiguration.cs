namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.ERP;

using Backend.Domain.ERP.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Name).HasMaxLength(200).IsRequired();
        builder.Property(w => w.Code).HasMaxLength(50).IsRequired();
        builder.Property(w => w.Address).HasMaxLength(500);
        builder.Property(w => w.City).HasMaxLength(100);
        builder.HasIndex(w => new { w.TenantId, w.Code }).IsUnique();
    }
}

public class WarehouseLocationConfiguration : IEntityTypeConfiguration<WarehouseLocation>
{
    public void Configure(EntityTypeBuilder<WarehouseLocation> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Code).HasMaxLength(80).IsRequired();
        builder.Property(l => l.Aisle).HasMaxLength(50);
        builder.Property(l => l.Rack).HasMaxLength(50);
        builder.Property(l => l.Level).HasMaxLength(50);
        builder.HasIndex(l => new { l.WarehouseId, l.Code }).IsUnique();
        builder.HasOne(l => l.Warehouse)
            .WithMany(w => w.WarehouseLocations)
            .HasForeignKey(l => l.WarehouseId);
    }
}

public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.LotNumber).HasMaxLength(100);
        builder.HasIndex(s => new { s.TenantId, s.ProductId, s.WarehouseId, s.WarehouseLocationId, s.LotNumber, s.ExpirationDate, s.Condition });
        builder.HasOne(s => s.Product).WithMany().HasForeignKey(s => s.ProductId);
        builder.HasOne(s => s.Warehouse).WithMany().HasForeignKey(s => s.WarehouseId);
        builder.HasOne(s => s.WarehouseLocation).WithMany().HasForeignKey(s => s.WarehouseLocationId);
    }
}

public class InventoryMovementConfiguration : IEntityTypeConfiguration<InventoryMovement>
{
    public void Configure(EntityTypeBuilder<InventoryMovement> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Property(m => m.Reference).HasMaxLength(100);
        builder.Property(m => m.Notes).HasMaxLength(500);
        builder.Property(m => m.Reason).HasMaxLength(200);
        builder.Property(m => m.LotNumber).HasMaxLength(100);
        builder.HasOne(m => m.Product).WithMany().HasForeignKey(m => m.ProductId);
        builder.HasOne(m => m.Warehouse).WithMany().HasForeignKey(m => m.WarehouseId);
        builder.HasOne(m => m.WarehouseLocation).WithMany().HasForeignKey(m => m.WarehouseLocationId);
    }
}
