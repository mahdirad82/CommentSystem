using CommentSystem.Api.Controllers.Base;
using CommentSystem.Application.DTOs;
using CommentSystem.Application.Interfaces;
using CommentSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CommentSystem.Api.Controllers;

[ApiController]
[Route("api/user/comments")]
public class UserCommentsController(ICommentService commentService, ICurrentUserService currentUser)
    : ApiControllerBase
{
    /// <summary>
    /// Gets comments for the current user.
    /// </summary>
    /// <returns>A list of user comments for the user.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserCommentDto>>> GetUserComments()
    {
        if (currentUser.Role != UserRole.User)
            return ForbiddenResult();

        var result = await commentService.GetCommentsForUserAsync(currentUser.UserId);

        return result.IsFailure ? NotFoundResult(result.Error) : Ok(result.Value);
    }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="dto">The DTO containing comment details.</param>
    /// <returns>No content if successful, or an error result.</returns>
    [HttpPost]
    public async Task<ActionResult> CreateComment([FromBody] CreateCommentDto dto)
    {
        if (currentUser.Role != UserRole.User)
            return ForbiddenResult();

        var result = await commentService.CreateCommentAsync(dto, currentUser.UserId);

        return result.IsFailure ? BadRequestResult(result.Error) : Created();
    }
}