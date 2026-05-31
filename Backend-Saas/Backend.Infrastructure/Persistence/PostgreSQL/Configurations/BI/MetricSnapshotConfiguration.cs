namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.BI;

using Backend.Domain.BI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MetricSnapshotConfiguration : IEntityTypeConfiguration<MetricSnapshot>
{
    public void Configure(EntityTypeBuilder<MetricSnapshot> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.MetricKey)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(m => m.MetricName)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.HasIndex(m => new { m.MetricKey, m.PeriodStart, m.PeriodEnd });
        
        builder.Property(m => m.Value)
            .HasPrecision(18, 4);
        
        builder.Property(m => m.Dimension)
            .HasMaxLength(200);
        
        builder.Property(m => m.SourceSystem)
            .HasMaxLength(100);
    }
}
