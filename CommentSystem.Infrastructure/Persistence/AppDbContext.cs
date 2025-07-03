using CommentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace CommentSystem.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Booking configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.Id);

            // optional one-to-one relationship
            entity.HasOne(b => b.Comment)
                .WithOne(c => c.Booking)
                .HasForeignKey<Comment>(c => c.BookingId);
        });

        // Comment configuration
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.BookingId).IsUnique();
            entity.Property(c => c.Content).IsRequired();
            entity.Property(c => c.Rating).IsRequired();
            entity.Property(c => c.Status).IsRequired();
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.Version).IsConcurrencyToken();
        });

        modelBuilder.Seed();
    }
}