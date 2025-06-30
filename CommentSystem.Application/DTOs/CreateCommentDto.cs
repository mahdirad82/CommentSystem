using System.ComponentModel.DataAnnotations;

namespace CommentSystem.Application.DTOs;

public record CreateCommentDto(
    [Required] int BookingId,
    
    [Required] [Length(1, 500)] string Content,
    
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    int Rating
);