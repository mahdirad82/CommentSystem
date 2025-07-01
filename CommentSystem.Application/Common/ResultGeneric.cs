namespace CommentSystem.Application.Common;

public class Result<T>
{
    private bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}