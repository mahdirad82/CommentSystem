using Bogus;
using CommentSystem.Domain.Entities;
using CommentSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CommentSystem.Infrastructure.Persistence;

public static class AppDbContextSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var faker = new Faker();
        var bookings = new List<Booking>();
        var comments = new List<Comment>();

        for (var i = 1; i <= 30; i++)
        {
            var bookingDate = faker.Date.Past();
            bookings.Add(new Booking
            {
                Id = i,
                UserId = faker.Random.Int(1, 10),
                HotelId = faker.Random.Int(1, 5),
                BookingDate = bookingDate
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
                    CreatedAt = faker.Date.Soon(10, bookingDate)
                });
            }
        }

        modelBuilder.Entity<Booking>().HasData(bookings);
        modelBuilder.Entity<Comment>().HasData(comments);
    }
}