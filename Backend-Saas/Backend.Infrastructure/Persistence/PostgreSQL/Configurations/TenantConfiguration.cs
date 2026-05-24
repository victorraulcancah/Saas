namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations;

using Backend.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Slug).HasMaxLength(100).IsRequired();
        builder.HasIndex(t => t.Slug).IsUnique();
        builder.Property(t => t.Email).HasMaxLength(200);
        builder.Property(t => t.Phone).HasMaxLength(50);
        builder.Property(t => t.SubscriptionPlan).HasMaxLength(100);
    }
}
