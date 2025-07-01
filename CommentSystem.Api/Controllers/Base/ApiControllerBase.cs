using Microsoft.AspNetCore.Mvc;

namespace CommentSystem.Api.Controllers.Base;

/// <summary>
/// Base controller for API endpoints, providing common functionality and error handling.
/// </summary>
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// Returns a 403 Forbidden result.
    /// </summary>
    /// <param name="detail">Optional: A detailed error message.</param>
    /// <returns>An <see cref="ActionResult"/> representing a 403 Forbidden response.</returns>
    protected ActionResult ForbiddenResult(string? detail = null) =>
        Problem(
            statusCode: StatusCodes.Status403Forbidden,
            title: "Forbidden",
            detail: detail ?? "You do not have permission to access this resource.");

    /// <summary>
    /// Returns a 400 Bad Request result.
    /// </summary>
    /// <param name="error">Optional: A detailed error message.</param>
    /// <returns>An <see cref="ActionResult"/> representing a 400 Bad Request response.</returns>
    protected ActionResult BadRequestResult(string? error = null) =>
        Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            detail: error ?? "The request was invalid.");

    /// <summary>
    /// Returns a 404 Not Found result.
    /// </summary>
    /// <param name="error">Optional: A detailed error message.</param>
    /// <returns>An <see cref="ActionResult"/> representing a 404 Not Found response.</returns>
    protected ActionResult NotFoundResult(string? error = null) =>
        Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: error ?? "The requested resource was not found.");
}