namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.Saas;

using Backend.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Key).HasMaxLength(200).IsRequired();
        builder.HasIndex(p => p.Key).IsUnique();
        builder.Property(p => p.Action).HasMaxLength(50).IsRequired();

        builder.HasOne(p => p.SaasSystem)
            .WithMany()
            .HasForeignKey(p => p.SaasSystemId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.SaasModule)
            .WithMany()
            .HasForeignKey(p => p.SaasModuleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.SaasSubModule)
            .WithMany()
            .HasForeignKey(p => p.SaasSubModuleId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
