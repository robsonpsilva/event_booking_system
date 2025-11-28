using Microsoft.EntityFrameworkCore;
using EventBookingSystem.Models;
namespace EventBookingSystem.Data
{
    // Database context class, inherits from EF Core's DbContext.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for the Events table.
        public DbSet<Event> Events { get; set; }
        
        // DbSet for the Ticket Types table.
        public DbSet<TicketType> TicketTypes { get; set; }

        // NEW: DbSet for the Registrations table.
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Event <-> TicketType Relationship
            modelBuilder.Entity<Event>()
                .HasMany(e => e.TicketTypes)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId)
                .IsRequired();

            // Registration <-> TicketType Relationship
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.TicketType)
                .WithMany() // No navigation property in TicketType for simplicity
                .HasForeignKey(r => r.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents accidental cascading deletion

            // Registration <-> Event Relationship
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Event)
                .WithMany()
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configures price precision.
            modelBuilder.Entity<TicketType>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}