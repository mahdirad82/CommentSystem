using CommentSystem.Api.Controllers.Base;
using CommentSystem.Application.DTOs;
using CommentSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommentSystem.Api.Controllers;

[ApiController]
[Route("api/hotels")]
public class HotelCommentsController(ICommentService commentService)
    : ApiControllerBase
{
    [HttpGet("{hotelId:int}/comments")]
    public async Task<ActionResult<IEnumerable<PublicCommentDto>>> GetApprovedComments(int hotelId)
    {
        var result = await commentService.GetApprovedCommentsForHotelAsync(hotelId);

        return result.IsFailure ? NotFoundResult(result.Error) : Ok(result.Value);
    }
}