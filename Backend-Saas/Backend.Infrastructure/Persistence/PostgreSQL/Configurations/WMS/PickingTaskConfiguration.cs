namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.WMS;

using Backend.Domain.WMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PickingTaskConfiguration : IEntityTypeConfiguration<PickingTask>
{
    public void Configure(EntityTypeBuilder<PickingTask> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.PickingNumber)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(p => p.PickingNumber)
            .IsUnique();
        
        builder.Property(p => p.OrderType)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(p => new { p.WarehouseId, p.Status });
        
        builder.HasMany(p => p.Items)
            .WithOne()
            .HasForeignKey(i => i.PickingTaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
