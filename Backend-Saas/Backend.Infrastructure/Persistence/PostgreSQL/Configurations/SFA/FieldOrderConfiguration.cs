namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.SFA;

using Backend.Domain.SFA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FieldOrderConfiguration : IEntityTypeConfiguration<FieldOrder>
{
    public void Configure(EntityTypeBuilder<FieldOrder> builder)
    {
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.OrderNumber)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(f => f.OrderNumber)
            .IsUnique();
        
        builder.HasIndex(f => new { f.CustomerId, f.VisitDate });
        
        builder.HasIndex(f => new { f.SalespersonId, f.Status });
        
        builder.Property(f => f.Notes)
            .HasMaxLength(1000);
    }
}
