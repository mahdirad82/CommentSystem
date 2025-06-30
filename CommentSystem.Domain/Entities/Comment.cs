using CommentSystem.Domain.Enums;

namespace CommentSystem.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public CommentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = default!;
}