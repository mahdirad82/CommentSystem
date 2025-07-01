using CommentSystem.Api.Controllers.Base;
using CommentSystem.Application.DTOs;
using CommentSystem.Application.Interfaces;
using CommentSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CommentSystem.Api.Controllers;

[ApiController]
[Route("api/admin/comments")]
public class AdminCommentsController(ICommentService commentService, ICurrentUserService currentUserService)
    : ApiControllerBase
{
    /// <summary>
    /// Gets all system comments, optionally filtered by status for Admins.
    /// </summary>
    /// <param name="status">The status of the comments to retrieve.</param>
    /// <returns>A list of admin comments.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdminCommentDto>>> GetAllSystemComments(
        [FromQuery] CommentStatus? status)
    {
        if (currentUserService.Role != UserRole.Admin)
            return ForbiddenResult();

        var result = await commentService.GetAllSystemCommentsAsync(status);

        return result.IsFailure ? NotFoundResult(result.Error) : Ok(result.Value);
    }

    /// <summary>
    /// Updates the status of a pending comment.
    /// </summary>
    /// <param name="id">The ID of the comment to update.</param>
    /// <param name="dto">The DTO containing the new status.</param>
    /// <returns>No content if successful, or an error result.</returns>
    [HttpPut("{id:int}/status")]
    public async Task<ActionResult> UpdateCommentStatus(int id, [FromBody] UpdateCommentStatusDto dto)
    {
        if (currentUserService.Role != UserRole.Admin)
            return ForbiddenResult();

        var result = await commentService.UpdateCommentStatusAsync(id, dto);
        if (result.IsFailure)
        {
            return result.Error switch
            {
                "Comment not found." => NotFoundResult(result.Error),
                _ => BadRequestResult(result.Error)
            };
        }

        return NoContent();
    }
}