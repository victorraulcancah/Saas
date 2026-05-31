namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.Saas;

using Backend.Domain.Saas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TenantCompanyProfileConfiguration : IEntityTypeConfiguration<TenantCompanyProfile>
{
    public void Configure(EntityTypeBuilder<TenantCompanyProfile> builder)
    {
        builder.HasKey(p => p.TenantId);
        builder.Property(p => p.Ruc).HasMaxLength(11).IsRequired();
        builder.Property(p => p.RazonSocial).HasMaxLength(200).IsRequired();
        builder.Property(p => p.NombreComercial).HasMaxLength(200);
        builder.Property(p => p.EmailFacturacion).HasMaxLength(200);
        builder.Property(p => p.Phone).HasMaxLength(50);
        builder.Property(p => p.DireccionFiscal).HasMaxLength(500).IsRequired();
        builder.Property(p => p.Ubigeo).HasMaxLength(6).IsRequired();
        builder.Property(p => p.Departamento).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Provincia).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Distrito).HasMaxLength(100).IsRequired();
        builder.Property(p => p.RepresentanteLegal).HasMaxLength(200);
        builder.Property(p => p.TipoContribuyente).HasMaxLength(100);
        builder.Property(p => p.RegimenTributario).HasMaxLength(100);
        builder.Property(p => p.EstadoSunat).HasMaxLength(100);
        builder.Property(p => p.CondicionSunat).HasMaxLength(100);
        builder.HasOne(p => p.Tenant).WithOne(t => t.CompanyProfile).HasForeignKey<TenantCompanyProfile>(p => p.TenantId);
    }
}

public class TenantBranchConfiguration : IEntityTypeConfiguration<TenantBranch>
{
    public void Configure(EntityTypeBuilder<TenantBranch> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).HasMaxLength(200).IsRequired();
        builder.Property(b => b.Code).HasMaxLength(50).IsRequired();
        builder.Property(b => b.SunatEstablishmentCode).HasMaxLength(10);
        builder.Property(b => b.Address).HasMaxLength(500).IsRequired();
        builder.Property(b => b.Ubigeo).HasMaxLength(6).IsRequired();
        builder.Property(b => b.Departamento).HasMaxLength(100).IsRequired();
        builder.Property(b => b.Provincia).HasMaxLength(100).IsRequired();
        builder.Property(b => b.Distrito).HasMaxLength(100).IsRequired();
        builder.HasIndex(b => new { b.TenantId, b.Code }).IsUnique();
        builder.HasOne(b => b.Tenant).WithMany(t => t.Branches).HasForeignKey(b => b.TenantId);
    }
}

public class TenantSunatCredentialConfiguration : IEntityTypeConfiguration<TenantSunatCredential>
{
    public void Configure(EntityTypeBuilder<TenantSunatCredential> builder)
    {
        builder.HasKey(c => c.TenantId);
        builder.Property(c => c.Environment).HasMaxLength(50).IsRequired();
        builder.Property(c => c.SendMode).HasMaxLength(50).IsRequired();
        builder.Property(c => c.SolUser).HasMaxLength(100);
        builder.Property(c => c.EncryptedSolPassword).HasMaxLength(1000);
        builder.Property(c => c.OseProvider).HasMaxLength(100);
        builder.Property(c => c.OseApiUrl).HasMaxLength(500);
        builder.Property(c => c.EncryptedOseToken).HasMaxLength(1000);
        builder.HasOne(c => c.Tenant).WithOne(t => t.SunatCredential).HasForeignKey<TenantSunatCredential>(c => c.TenantId);
    }
}

public class TenantCertificateConfiguration : IEntityTypeConfiguration<TenantCertificate>
{
    public void Configure(EntityTypeBuilder<TenantCertificate> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Format).HasMaxLength(20).IsRequired();
        builder.Property(c => c.CertificateContent).HasColumnType("bytea").IsRequired();
        builder.Property(c => c.EncryptedPassword).HasMaxLength(1000);
        builder.HasOne(c => c.Tenant).WithMany(t => t.Certificates).HasForeignKey(c => c.TenantId);
    }
}

public class InvoiceSeriesConfiguration : IEntityTypeConfiguration<InvoiceSeries>
{
    public void Configure(EntityTypeBuilder<InvoiceSeries> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.DocumentType).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Series).HasMaxLength(10).IsRequired();
        builder.HasIndex(s => new { s.TenantId, s.BranchId, s.DocumentType, s.Series }).IsUnique();
        builder.HasOne(s => s.Tenant).WithMany(t => t.InvoiceSeries).HasForeignKey(s => s.TenantId);
        builder.HasOne(s => s.Branch).WithMany(b => b.InvoiceSeries).HasForeignKey(s => s.BranchId);
    }
}
