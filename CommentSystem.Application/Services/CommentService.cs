using AutoMapper;
using CommentSystem.Application.Common;
using CommentSystem.Application.DTOs;
using CommentSystem.Application.Interfaces;
using CommentSystem.Domain.Entities;
using CommentSystem.Domain.Enums;

namespace CommentSystem.Application.Services;

public class CommentService(ICommentRepository commentRepository, IMapper mapper) : ICommentService
{
    public async Task<Result> CreateCommentAsync(CreateCommentDto dto, int userId)
    {
        if (await commentRepository.GetBookingAvailableForCommentAsync(dto.BookingId, userId) is null)
            return Result.Failure(
                "This booking either does not exist, does not belong to you, or already has a comment associated with it.");

        var comment = mapper.Map<Comment>(dto);

        await commentRepository.AddAsync(comment);
        await commentRepository.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<IEnumerable<PublicCommentDto>>> GetApprovedCommentsForHotelAsync(
        int hotelId)
    {
        var comments = await commentRepository.GetByHotelIdAndStatusAsync(hotelId, CommentStatus.Approved);
        if (!comments.Any())
            return Result<IEnumerable<PublicCommentDto>>.Failure(
                "No approved comments found for this hotel.");

        var publicComments = mapper.Map<IEnumerable<PublicCommentDto>>(comments);
        return Result<IEnumerable<PublicCommentDto>>.Success(publicComments);
    }

    public async Task<Result<IEnumerable<UserCommentDto>>> GetCommentsForUserAsync(int userId)
    {
        var comments = await commentRepository.GetByUserIdAsync(userId);
        if (!comments.Any())
            return Result<IEnumerable<UserCommentDto>>.Failure("No comments found for this user.");

        var userComments = mapper.Map<IEnumerable<UserCommentDto>>(comments);
        return Result<IEnumerable<UserCommentDto>>.Success(userComments);
    }

    public async Task<Result<IEnumerable<AdminCommentDto>>> GetAllSystemCommentsAsync(
        CommentStatus? status)
    {
        var comments = await commentRepository.GetAllAsync(status);
        if (!comments.Any())
            return Result<IEnumerable<AdminCommentDto>>.Failure("No comments found.");

        var systemComments = mapper.Map<IEnumerable<AdminCommentDto>>(comments);
        return Result<IEnumerable<AdminCommentDto>>.Success(systemComments);
    }

    public async Task<Result> UpdateCommentStatusAsync(int commentId, UpdateCommentStatusDto dto)
    {
        var comment = await commentRepository.GetByIdAsync(commentId);
        if (comment is null)
            return Result.Failure("Comment not found.");

        if (comment.Status != CommentStatus.Pending)
            return Result.Failure("Comment is not pending.");
        
        if (dto.NewStatus is not (CommentStatus.Approved or CommentStatus.Rejected))
            return Result.Failure("New Status is not approved or not rejected.");

        comment.Status = dto.NewStatus;
        await commentRepository.SaveChangesAsync();
        return Result.Success();
    }
}