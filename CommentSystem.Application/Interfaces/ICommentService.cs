using CommentSystem.Application.Common;
using CommentSystem.Application.DTOs;
using CommentSystem.Domain.Enums;

namespace CommentSystem.Application.Interfaces;

public interface ICommentService
{
    Task<Result> CreateCommentAsync(CreateCommentDto dto, int userId);

    Task<Result<IEnumerable<PublicCommentDto>>> GetApprovedCommentsForHotelAsync(int hotelId);

    Task<Result<IEnumerable<UserCommentDto>>> GetCommentsForUserAsync(int userId);

    Task<Result<IEnumerable<AdminCommentDto>>> GetAllSystemCommentsAsync(CommentStatus? status);

    Task<Result> UpdateCommentStatusAsync(int commentId, UpdateCommentStatusDto dto);
}