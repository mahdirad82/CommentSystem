using CommentSystem.Domain.Enums;

namespace CommentSystem.Application.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }
    UserRole Role { get; }
}