using CommentSystem.Domain.Enums;

namespace CommentSystem.Domain.Entities;

public class Comment
{
    public int Id { get; init; }
    public required string Content { get; init; }
    public int Rating { get; init; }
    public CommentStatus Status { get; set; } = CommentStatus.Pending;
    public DateTime CreatedAt { get; init; }

    public int BookingId { get; init; }
    public Booking Booking { get; init; } = null!;
}