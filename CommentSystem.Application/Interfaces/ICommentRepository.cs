using CommentSystem.Domain.Entities;
using CommentSystem.Domain.Enums;

namespace CommentSystem.Application.Interfaces;

public interface ICommentRepository
{
    Task AddAsync(Comment comment);
    Task<Comment?> GetByIdAsync(int id);
    Task<IEnumerable<Comment>> GetByHotelIdAndStatusAsync(int hotelId, CommentStatus status = CommentStatus.Approved);
    Task<IEnumerable<Comment>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Comment>> GetAllAsync(CommentStatus? status = null);
    Task<Booking?> GetBookingAvailableForCommentAsync(int bookingId, int userId);
    Task<int> SaveChangesAsync();
}