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
        builder.Property(t => t.Ruc).HasMaxLength(11).IsRequired();
        builder.HasIndex(t => t.Ruc).IsUnique();
        builder.Property(t => t.RazonSocial).HasMaxLength(200).IsRequired();
        builder.Property(t => t.NombreComercial).HasMaxLength(200);
        builder.Property(t => t.Email).HasMaxLength(200);
        builder.Property(t => t.EmailFacturacion).HasMaxLength(200);
        builder.Property(t => t.Phone).HasMaxLength(50);
        builder.Property(t => t.TelefonoSecundario).HasMaxLength(50);
        builder.Property(t => t.DireccionFiscal).HasMaxLength(500);
        builder.Property(t => t.Ubigeo).HasMaxLength(6);
        builder.Property(t => t.Departamento).HasMaxLength(100);
        builder.Property(t => t.Provincia).HasMaxLength(100);
        builder.Property(t => t.Distrito).HasMaxLength(100);
        builder.Property(t => t.Website).HasMaxLength(500);
        builder.Property(t => t.LogoBase64).HasColumnType("text");
        builder.Property(t => t.ClaveSol).HasMaxLength(500);
        builder.Property(t => t.CertificadoPem).HasColumnType("bytea");
        builder.Property(t => t.CertificadoPassword).HasMaxLength(500);
        builder.Property(t => t.SubscriptionPlan).HasMaxLength(100);
    }
}
