namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.Saas;

using Backend.Domain.Saas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PlanSystemConfiguration : IEntityTypeConfiguration<PlanSystem>
{
    public void Configure(EntityTypeBuilder<PlanSystem> builder)
    {
        builder.HasKey(ps => new { ps.PlanId, ps.SystemId });
        builder.HasOne(ps => ps.Plan).WithMany(p => p.Systems).HasForeignKey(ps => ps.PlanId);
        builder.HasOne(ps => ps.System).WithMany(s => s.PlanSystems).HasForeignKey(ps => ps.SystemId);
    }
}

public class PlanModuleConfiguration : IEntityTypeConfiguration<PlanModule>
{
    public void Configure(EntityTypeBuilder<PlanModule> builder)
    {
        builder.HasKey(pm => new { pm.PlanId, pm.ModuleId });
        builder.HasOne(pm => pm.Plan).WithMany(p => p.Modules).HasForeignKey(pm => pm.PlanId);
        builder.HasOne(pm => pm.Module).WithMany(m => m.PlanModules).HasForeignKey(pm => pm.ModuleId);
    }
}

public class PlanSubModuleConfiguration : IEntityTypeConfiguration<PlanSubModule>
{
    public void Configure(EntityTypeBuilder<PlanSubModule> builder)
    {
        builder.HasKey(psm => new { psm.PlanId, psm.SubModuleId });
        builder.HasOne(psm => psm.Plan).WithMany(p => p.SubModules).HasForeignKey(psm => psm.PlanId);
        builder.HasOne(psm => psm.SubModule).WithMany(sm => sm.PlanSubModules).HasForeignKey(psm => psm.SubModuleId);
    }
}

public class TenantSubscriptionConfiguration : IEntityTypeConfiguration<TenantSubscription>
{
    public void Configure(EntityTypeBuilder<TenantSubscription> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Status).HasMaxLength(50).IsRequired();
        builder.HasIndex(s => new { s.TenantId, s.Status });
        builder.HasOne(s => s.Tenant).WithMany(t => t.Subscriptions).HasForeignKey(s => s.TenantId);
        builder.HasOne(s => s.Plan).WithMany(p => p.Subscriptions).HasForeignKey(s => s.PlanId);
    }
}

public class TenantSystemLicenseConfiguration : IEntityTypeConfiguration<TenantSystemLicense>
{
    public void Configure(EntityTypeBuilder<TenantSystemLicense> builder)
    {
        builder.HasKey(l => new { l.TenantId, l.SystemId });
        builder.Property(l => l.Source).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Config).HasColumnType("jsonb");
        builder.HasOne(l => l.Tenant).WithMany(t => t.SystemLicenses).HasForeignKey(l => l.TenantId);
        builder.HasOne(l => l.System).WithMany(s => s.TenantLicenses).HasForeignKey(l => l.SystemId);
    }
}

public class TenantModuleLicenseConfiguration : IEntityTypeConfiguration<TenantModuleLicense>
{
    public void Configure(EntityTypeBuilder<TenantModuleLicense> builder)
    {
        builder.HasKey(l => new { l.TenantId, l.ModuleId });
        builder.Property(l => l.Source).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Config).HasColumnType("jsonb");
        builder.HasOne(l => l.Tenant).WithMany(t => t.ModuleLicenses).HasForeignKey(l => l.TenantId);
        builder.HasOne(l => l.Module).WithMany(m => m.TenantLicenses).HasForeignKey(l => l.ModuleId);
    }
}

public class TenantSubModuleLicenseConfiguration : IEntityTypeConfiguration<TenantSubModuleLicense>
{
    public void Configure(EntityTypeBuilder<TenantSubModuleLicense> builder)
    {
        builder.HasKey(l => new { l.TenantId, l.SubModuleId });
        builder.Property(l => l.Source).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Config).HasColumnType("jsonb");
        builder.HasOne(l => l.Tenant).WithMany(t => t.SubModuleLicenses).HasForeignKey(l => l.TenantId);
        builder.HasOne(l => l.SubModule).WithMany(sm => sm.TenantLicenses).HasForeignKey(l => l.SubModuleId);
    }
}
