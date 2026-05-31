namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.CRM;

using Backend.Domain.CRM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OpportunityConfiguration : IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Title).HasMaxLength(200).IsRequired();
        builder.Property(o => o.Description).HasMaxLength(1000);
        builder.Property(o => o.Amount).HasColumnType("decimal(18,2)");
        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.Notes).HasMaxLength(1000);
        builder.HasIndex(o => new { o.TenantId, o.OrderNumber }).IsUnique();
        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(o => o.Items)
            .WithOne(i => i.SalesOrder)
            .HasForeignKey(i => i.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SalesOrderItemConfiguration : IEntityTypeConfiguration<SalesOrderItem>
{
    public void Configure(EntityTypeBuilder<SalesOrderItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Property(i => i.TotalPrice).HasColumnType("decimal(18,2)");
    }
}

public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Subject).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(2000);
        builder.Property(t => t.AssignedTo).HasMaxLength(200);
        builder.Property(t => t.Resolution).HasMaxLength(2000);
        builder.HasOne(t => t.Customer)
            .WithMany()
            .HasForeignKey(t => t.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
