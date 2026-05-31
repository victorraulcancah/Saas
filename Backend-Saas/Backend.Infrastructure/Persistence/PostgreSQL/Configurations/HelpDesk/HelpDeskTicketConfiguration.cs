namespace Backend.Infrastructure.Persistence.PostgreSQL.Configurations.HelpDesk;

using Backend.Domain.HelpDesk.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class HelpDeskTicketConfiguration : IEntityTypeConfiguration<HelpDeskTicket>
{
    public void Configure(EntityTypeBuilder<HelpDeskTicket> builder)
    {
        builder.HasKey(h => h.Id);
        
        builder.Property(h => h.TicketNumber)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(h => h.TicketNumber)
            .IsUnique();
        
        builder.Property(h => h.Subject)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(h => h.Description)
            .HasMaxLength(2000);
        
        builder.Property(h => h.Channel)
            .HasMaxLength(50);
        
        builder.Property(h => h.AssignedTo)
            .HasMaxLength(200);
        
        builder.HasIndex(h => new { h.Status, h.Priority });
        
        builder.Property(h => h.Resolution)
            .HasMaxLength(2000);
    }
}
