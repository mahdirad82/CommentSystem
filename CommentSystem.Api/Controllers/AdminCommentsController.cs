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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdminCommentDto>>> GetAllSystemComments(
        [FromQuery] CommentStatus? status)
    {
        if (currentUserService.Role != UserRole.Admin)
            return ForbiddenResult();

        var result = await commentService.GetAllSystemCommentsAsync(status);

        return result.IsFailure ? NotFoundResult(result.Error) : Ok(result.Value);
    }

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