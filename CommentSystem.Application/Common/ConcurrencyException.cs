namespace CommentSystem.Application.Common;

public class ConcurrencyException(string message) : Exception(message);