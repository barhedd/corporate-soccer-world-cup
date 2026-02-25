namespace CorporateSoccerWorldCup.Application.Common.Results;

public class Result
{
    public bool Success { get; }
    public bool Failure => !Success;
    public string? ErrorMessage { get; }
    public string? ErrorCode { get; }

    protected Result(bool success, string? errorMessage, string? errorCode)
    {
        Success = success;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public static Result Ok() => new(true, null, null);

    public static Result Fail(string message, string? code = null)
        => new(false, message, code);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool success, T? value, string? message, string? code)
        : base(success, message, code)
    {
        Value = value;
    }

    public static Result<T> Ok(T value)
        => new(true, value, null, null);

    public new static Result<T> Fail(string message, string? code = null)
        => new(false, default, message, code);
}
