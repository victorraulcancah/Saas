namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.HR;

using Backend.Domain.HR.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(e => e.LastName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(e => e.Email)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.HasIndex(e => e.Email);
        
        builder.Property(e => e.Phone)
            .HasMaxLength(50);
        
        builder.Property(e => e.DocumentNumber)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.HasIndex(e => e.DocumentNumber);
        
        builder.Property(e => e.Position)
            .HasMaxLength(100);
        
        builder.Property(e => e.Department)
            .HasMaxLength(100);
    }
}
