namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.LossPrevention;

using Backend.Domain.LossPrevention.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CycleCountConfiguration : IEntityTypeConfiguration<CycleCount>
{
    public void Configure(EntityTypeBuilder<CycleCount> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.CountNumber)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(c => c.CountNumber)
            .IsUnique();
        
        builder.HasIndex(c => new { c.WarehouseId, c.ScheduledDate });
        
        builder.Property(c => c.Category)
            .HasMaxLength(100);
        
        builder.Property(c => c.Notes)
            .HasMaxLength(1000);
    }
}
