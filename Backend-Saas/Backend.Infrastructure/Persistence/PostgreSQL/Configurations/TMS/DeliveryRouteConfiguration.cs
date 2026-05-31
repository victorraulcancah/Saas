namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.TMS;

using Backend.Domain.TMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DeliveryRouteConfiguration : IEntityTypeConfiguration<DeliveryRoute>
{
    public void Configure(EntityTypeBuilder<DeliveryRoute> builder)
    {
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.RouteNumber)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(d => d.RouteNumber)
            .IsUnique();
        
        builder.Property(d => d.VehiclePlate)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(d => d.DriverName)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.HasIndex(d => new { d.PlannedDate, d.Status });
        
        builder.Property(d => d.Notes)
            .HasMaxLength(1000);
    }
}
