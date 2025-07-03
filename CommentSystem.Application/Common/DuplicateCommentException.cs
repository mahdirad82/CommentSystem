namespace CommentSystem.Application.Common;

public class DuplicateCommentException(string message) : Exception(message);