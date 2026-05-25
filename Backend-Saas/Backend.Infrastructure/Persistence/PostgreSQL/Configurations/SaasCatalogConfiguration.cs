namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations;

using Backend.Domain.Saas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SaasSystemConfiguration : IEntityTypeConfiguration<SaasSystem>
{
    public void Configure(EntityTypeBuilder<SaasSystem> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Key).HasMaxLength(100).IsRequired();
        builder.HasIndex(s => s.Key).IsUnique();
        builder.Property(s => s.Icon).HasMaxLength(100);
        builder.Property(s => s.BasePath).HasMaxLength(200);
    }
}

public class SaasModuleConfiguration : IEntityTypeConfiguration<SaasModule>
{
    public void Configure(EntityTypeBuilder<SaasModule> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).HasMaxLength(200).IsRequired();
        builder.Property(m => m.Key).HasMaxLength(100).IsRequired();
        builder.HasIndex(m => new { m.SystemId, m.Key }).IsUnique();
        builder.Property(m => m.Icon).HasMaxLength(100);
        builder.Property(m => m.BasePath).HasMaxLength(200);
        builder.HasOne(m => m.System)
            .WithMany(s => s.Modules)
            .HasForeignKey(m => m.SystemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SaasSubModuleConfiguration : IEntityTypeConfiguration<SaasSubModule>
{
    public void Configure(EntityTypeBuilder<SaasSubModule> builder)
    {
        builder.HasKey(sm => sm.Id);
        builder.Property(sm => sm.Name).HasMaxLength(200).IsRequired();
        builder.Property(sm => sm.Key).HasMaxLength(100).IsRequired();
        builder.HasIndex(sm => new { sm.ModuleId, sm.Key }).IsUnique();
        builder.Property(sm => sm.RoutePath).HasMaxLength(200);
        builder.HasOne(sm => sm.Module)
            .WithMany(m => m.SubModules)
            .HasForeignKey(sm => sm.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SaasPlanConfiguration : IEntityTypeConfiguration<SaasPlan>
{
    public void Configure(EntityTypeBuilder<SaasPlan> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Key).HasMaxLength(100).IsRequired();
        builder.HasIndex(p => p.Key).IsUnique();
        builder.Property(p => p.Price).HasPrecision(18, 2);
        builder.Property(p => p.Currency).HasMaxLength(3).IsRequired();
        builder.Property(p => p.BillingCycle).HasMaxLength(50).IsRequired();
    }
}
