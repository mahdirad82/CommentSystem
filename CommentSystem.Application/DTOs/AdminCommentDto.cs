namespace CommentSystem.Application.DTOs;

public record AdminCommentDto(
    int Id,
    string Content,
    int Rating,
    DateTime CreatedAt,
    int BookingId,
    string Status
);