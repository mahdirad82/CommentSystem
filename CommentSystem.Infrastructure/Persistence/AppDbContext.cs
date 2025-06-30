using Bogus;
using CommentSystem.Domain.Entities;
using CommentSystem.Domain.Enums;
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
            entity.Property(c => c.Content).IsRequired();
            entity.Property(c => c.Rating).IsRequired();
            entity.Property(c => c.Status).IsRequired();
            entity.Property(c => c.CreatedAt).IsRequired();
        });
        
        var faker = new Faker();
        var bookings = new List<Booking>();
        var comments = new List<Comment>();

        for (var i = 1; i <= 30; i++)
        {
            bookings.Add(new Booking
            {
                Id = i,
                UserId = faker.Random.Int(1, 10),
                HotelId = faker.Random.Int(1, 5),
                BookingDate = faker.Date.Past(1)
            });

            if (i % 2 == 0)
            {
                comments.Add(new Comment
                {
                    Id = i,
                    BookingId = i,
                    Content = faker.Lorem.Paragraph(faker.Random.Int(1, 6)),
                    Rating = faker.Random.Int(1, 5),
                    Status = CommentStatus.Pending,
                    CreatedAt = faker.Date.Recent(60)
                });
            }
        }

        modelBuilder.Entity<Booking>().HasData(bookings);
        modelBuilder.Entity<Comment>().HasData(comments);
    }
}