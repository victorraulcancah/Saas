namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.ERP;

using Backend.Domain.ERP.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.InvoiceNumber).HasMaxLength(50);
        builder.Property(i => i.Series).HasMaxLength(10);
        builder.Property(i => i.Correlative).HasMaxLength(20);
        builder.Property(i => i.CustomerName).HasMaxLength(200);
        builder.Property(i => i.CustomerTaxId).HasMaxLength(20);
        builder.Property(i => i.Currency).HasMaxLength(10);
        builder.Property(i => i.CdrCode).HasMaxLength(50);
        builder.Property(i => i.CdrDescription).HasMaxLength(500);
        builder.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(i => i.TaxAmount).HasColumnType("decimal(18,2)");
    }
}
