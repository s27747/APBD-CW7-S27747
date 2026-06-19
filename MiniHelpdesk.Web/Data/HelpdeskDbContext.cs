using Microsoft.EntityFrameworkCore;
using MiniHelpdesk.Web.Models;

namespace MiniHelpdesk.Web.Data;

public class HelpdeskDbContext : DbContext
{
    public HelpdeskDbContext(DbContextOptions<HelpdeskDbContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(ticket => ticket.Id);

            entity.Property(ticket => ticket.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(ticket => ticket.Description)
                .HasMaxLength(1000);

            entity.Property(ticket => ticket.Status)
                .IsRequired();

            entity.Property(ticket => ticket.CreatedAt)
                .IsRequired();

            entity.HasMany(ticket => ticket.Comments)
                .WithOne(comment => comment.Ticket)
                .HasForeignKey(comment => comment.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TicketComment>(entity =>
        {
            entity.HasKey(comment => comment.Id);

            entity.Property(comment => comment.Author)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(comment => comment.Content)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(comment => comment.CreatedAt)
                .IsRequired();
        });
    }
}