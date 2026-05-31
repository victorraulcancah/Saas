namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.RetailAnalytics;

using Backend.Domain.RetailAnalytics.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StoreTrafficReadingConfiguration : IEntityTypeConfiguration<StoreTrafficReading>
{
    public void Configure(EntityTypeBuilder<StoreTrafficReading> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.HasIndex(s => new { s.BranchId, s.ReadingAt });
        
        builder.Property(s => s.Source)
            .HasMaxLength(100);
        
        builder.Property(s => s.SalesAmount)
            .HasPrecision(18, 2);
    }
}
