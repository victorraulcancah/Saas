namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations;

using Backend.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).HasMaxLength(200).IsRequired();
        builder.Property(m => m.Key).HasMaxLength(100).IsRequired();
        builder.HasIndex(m => m.Key).IsUnique();
        builder.Property(m => m.Icon).HasMaxLength(100);
        builder.Property(m => m.BasePath).HasMaxLength(200);
    }
}
