using CommentSystem.Domain.Enums;

namespace CommentSystem.Domain.Entities;

public class Comment
{
    public int Id { get; init; }
    public required string Content { get; init; }
    public int Rating { get; init; }
    public CommentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public int BookingId { get; init; }
    public Booking Booking { get; init; } = null!;
}