using CommentSystem.Api.Controllers.Base;
using CommentSystem.Application.DTOs;
using CommentSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommentSystem.Api.Controllers;

/// <summary>
/// Manages comments related to hotels.
/// </summary>
[ApiController]
[Route("api/hotels")]
public class HotelCommentsController(ICommentService commentService)
    : ApiControllerBase
{
    /// <summary>
    /// Gets approved comments for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <returns>A list of public comments.</returns>
    [HttpGet("{hotelId:int}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PublicCommentDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<PublicCommentDto>>> GetApprovedComments(int hotelId)
    {
        var result = await commentService.GetApprovedCommentsForHotelAsync(hotelId);

        return result.IsFailure ? NotFoundResult(result.Error) : Ok(result.Value);
    }
}