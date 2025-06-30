using Microsoft.AspNetCore.Mvc;

namespace CommentSystem.Api.Controllers.Base;

public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult ForbiddenResult(string? detail = null) =>
        Problem(
            statusCode: StatusCodes.Status403Forbidden,
            title: "Forbidden",
            detail: detail ?? "You do not have permission to access this resource.");

    protected ActionResult BadRequestResult(string? error = null) =>
        Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            detail: error ?? "The request was invalid.");

    protected ActionResult NotFoundResult(string? error = null) =>
        Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: error ?? "The requested resource was not found.");
}