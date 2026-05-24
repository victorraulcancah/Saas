namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations;

using Backend.Domain.CRM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(200);
        builder.Property(c => c.TaxId).HasMaxLength(20);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.Address).HasMaxLength(500);
        builder.Property(c => c.CreditLimit).HasColumnType("decimal(18,2)");
    }
}
