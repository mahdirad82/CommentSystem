using CommentSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommentSystem.Application.DTOs;

public record UpdateCommentStatusDto(
    [Required] CommentStatus NewStatus
);