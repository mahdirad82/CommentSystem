namespace CommentSystem.Application.DTOs;

// برای نمایش اطلاعات کامنت به کلاینت
public record AdminCommentDto(
    int Id,
    string Content,
    int Rating,
    DateTime CreatedAt,
    int BookingId,
    string Status
);