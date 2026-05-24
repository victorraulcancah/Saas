namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations;

using Backend.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TenantModuleConfiguration : IEntityTypeConfiguration<TenantModule>
{
    public void Configure(EntityTypeBuilder<TenantModule> builder)
    {
        builder.HasKey(tm => new { tm.TenantId, tm.ModuleId });
        builder.Property(tm => tm.Config).HasColumnType("jsonb");
        builder.HasOne(tm => tm.Tenant)
            .WithMany(t => t.TenantModules)
            .HasForeignKey(tm => tm.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(tm => tm.Module)
            .WithMany(m => m.TenantModules)
            .HasForeignKey(tm => tm.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
