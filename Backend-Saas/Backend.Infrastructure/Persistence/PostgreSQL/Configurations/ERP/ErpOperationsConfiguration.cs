namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.ERP;

using Backend.Domain.ERP.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
{
    public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.ReceiptNumber).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Notes).HasMaxLength(500);
        builder.HasIndex(r => new { r.TenantId, r.ReceiptNumber }).IsUnique();
        builder.HasOne(r => r.PurchaseOrder).WithMany().HasForeignKey(r => r.PurchaseOrderId);
        builder.HasOne(r => r.Warehouse).WithMany().HasForeignKey(r => r.WarehouseId);
        builder.HasMany(r => r.Items).WithOne(i => i.GoodsReceipt).HasForeignKey(i => i.GoodsReceiptId);
    }
}

public class GoodsReceiptItemConfiguration : IEntityTypeConfiguration<GoodsReceiptItem>
{
    public void Configure(EntityTypeBuilder<GoodsReceiptItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
        builder.Property(i => i.LotNumber).HasMaxLength(100);
        builder.Property(i => i.UnitCost).HasColumnType("decimal(18,2)");
    }
}

public class WarehouseTransferConfiguration : IEntityTypeConfiguration<WarehouseTransfer>
{
    public void Configure(EntityTypeBuilder<WarehouseTransfer> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.TransferNumber).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Notes).HasMaxLength(500);
        builder.HasIndex(t => new { t.TenantId, t.TransferNumber }).IsUnique();
        builder.HasOne(t => t.SourceWarehouse).WithMany().HasForeignKey(t => t.SourceWarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.TargetWarehouse).WithMany().HasForeignKey(t => t.TargetWarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(t => t.Items).WithOne(i => i.WarehouseTransfer).HasForeignKey(i => i.WarehouseTransferId);
    }
}

public class WarehouseTransferItemConfiguration : IEntityTypeConfiguration<WarehouseTransferItem>
{
    public void Configure(EntityTypeBuilder<WarehouseTransferItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
        builder.Property(i => i.LotNumber).HasMaxLength(100);
    }
}

public class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
{
    public void Configure(EntityTypeBuilder<StockAdjustment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.AdjustmentNumber).HasMaxLength(50).IsRequired();
        builder.Property(a => a.LotNumber).HasMaxLength(100);
        builder.Property(a => a.Reason).HasMaxLength(300);
        builder.HasIndex(a => new { a.TenantId, a.AdjustmentNumber }).IsUnique();
        builder.HasOne(a => a.Product).WithMany().HasForeignKey(a => a.ProductId);
        builder.HasOne(a => a.Warehouse).WithMany().HasForeignKey(a => a.WarehouseId);
    }
}

public class DispatchGuideConfiguration : IEntityTypeConfiguration<DispatchGuide>
{
    public void Configure(EntityTypeBuilder<DispatchGuide> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Series).HasMaxLength(10).IsRequired();
        builder.Property(g => g.Correlative).HasMaxLength(20).IsRequired();
        builder.Property(g => g.GuideNumber).HasMaxLength(50).IsRequired();
        builder.Property(g => g.ReasonCode).HasMaxLength(20);
        builder.Property(g => g.ReasonDescription).HasMaxLength(200);
        builder.Property(g => g.DestinationAddress).HasMaxLength(500);
        builder.Property(g => g.DestinationUbigeo).HasMaxLength(6);
        builder.Property(g => g.TransportistName).HasMaxLength(200);
        builder.Property(g => g.TransportistDocument).HasMaxLength(20);
        builder.HasIndex(g => new { g.TenantId, g.Series, g.Correlative }).IsUnique();
        builder.HasOne(g => g.SourceWarehouse).WithMany().HasForeignKey(g => g.SourceWarehouseId);
        builder.HasMany(g => g.Items).WithOne(i => i.DispatchGuide).HasForeignKey(i => i.DispatchGuideId);
    }
}

public class DispatchGuideItemConfiguration : IEntityTypeConfiguration<DispatchGuideItem>
{
    public void Configure(EntityTypeBuilder<DispatchGuideItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
        builder.Property(i => i.UnitOfMeasure).HasMaxLength(20).IsRequired();
    }
}
