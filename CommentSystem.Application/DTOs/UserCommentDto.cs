namespace CommentSystem.Application.DTOs;

public record UserCommentDto(
    int Id,
    string Content,
    int Rating,
    DateTime CreatedAt,
    int BookingId,
    string Status
);