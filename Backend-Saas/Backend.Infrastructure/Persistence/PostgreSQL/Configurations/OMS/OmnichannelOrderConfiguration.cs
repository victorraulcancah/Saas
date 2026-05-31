namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.OMS;

using Backend.Domain.OMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OmnichannelOrderConfiguration : IEntityTypeConfiguration<OmnichannelOrder>
{
    public void Configure(EntityTypeBuilder<OmnichannelOrder> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.OrderNumber)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(o => o.OrderNumber)
            .IsUnique();
        
        builder.Property(o => o.Channel)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(o => o.Channel);
        
        builder.Property(o => o.ExternalOrderNumber)
            .HasMaxLength(100);
        
        builder.Property(o => o.DeliveryMethod)
            .HasMaxLength(100);
        
        builder.Property(o => o.DeliveryAddress)
            .HasMaxLength(500);
        
        builder.Property(o => o.Notes)
            .HasMaxLength(1000);
        
        builder.Property(o => o.TotalAmount)
            .HasPrecision(18, 2);
        
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OmnichannelOrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(o => o.FulfillmentAssignments)
            .WithOne()
            .HasForeignKey(f => f.OmnichannelOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
