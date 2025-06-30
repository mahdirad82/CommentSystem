using CommentSystem.Application.Interfaces;
using CommentSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CommentSystem.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public int UserId { get; }
    public UserRole Role { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;

        var userIdHeader = context?.Request.Headers["UserId"].FirstOrDefault();
        var roleHeader = context?.Request.Headers["Role"].FirstOrDefault();

        if (!int.TryParse(userIdHeader, out var userId))
            throw new UnauthorizedAccessException("Invalid or missing user ID");

        if (!Enum.TryParse<UserRole>(roleHeader, ignoreCase: true, out var role))
            throw new UnauthorizedAccessException("Invalid or missing user role");

        UserId = userId;
        Role = role;
    }
}