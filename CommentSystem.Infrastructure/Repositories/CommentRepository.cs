using CommentSystem.Application.Interfaces;
using CommentSystem.Domain.Entities;
using CommentSystem.Domain.Enums;
using CommentSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommentSystem.Infrastructure.Repositories;

// پیاده‌سازی اینترفیس ریپازیتوری با استفاده از EF Core
public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task AddAsync(Comment comment) => await context.Comments.AddAsync(comment);

    public async Task<Comment?> GetByIdAsync(int id) => await context.Comments.FindAsync(id);

    public async Task<IEnumerable<Comment>> GetByHotelIdAndStatusAsync(int hotelId,
        CommentStatus status = CommentStatus.Approved)
    {
        return await context.Comments.AsNoTracking().Include(c => c.Booking)
            .Where(c => c.Booking.HotelId == hotelId && c.Status == status).ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetByUserIdAsync(int userId)
    {
        return await context.Comments.AsNoTracking().Include(c => c.Booking)
            .Where(c => c.Booking.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetAllAsync() =>
        await context.Comments.AsNoTracking().ToListAsync();

    public async Task<bool> IsBookingAvailableForCommentAsync(int bookingId, int userId)
    {
        return await context.Bookings.AnyAsync(b =>
            b.Id == bookingId &&
            b.UserId == userId &&
            b.Comment == null);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}