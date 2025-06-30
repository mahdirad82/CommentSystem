namespace CommentSystem.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }

    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);

    public void ThrowIfFailed()
    {
        if (IsFailure)
            throw new InvalidOperationException(Error ?? "An unknown error occurred.");
    }
}