namespace CommentSystem.Application.DTOs;

public record PublicCommentDto(
    int Id,
    string Content,
    int Rating,
    DateTime CreatedAt,
    int BookingId
);