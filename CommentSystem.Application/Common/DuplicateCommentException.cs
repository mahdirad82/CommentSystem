namespace CommentSystem.Application.Common;

public class DuplicateCommentException : Exception
{
    public DuplicateCommentException(string message) : base(message)
    {
    }
}