namespace ClinicSaas.Application.Common;

public sealed record Error(string Message, string Code = "validation");

public class Result
{
    protected Result(bool isSuccess, IReadOnlyList<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors { get; }

    public static Result Success() => new(true, Array.Empty<Error>());
    public static Result Ok() => Success();
    public static Result Failure(params Error[] errors) => new(false, errors);
    public static Result Failure(params string[] messages) =>
        new(false, messages.Select(message => new Error(message)).ToArray());
}

public sealed class Result<T> : Result
{
    private Result(bool isSuccess, T? value, IReadOnlyList<Error> errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new(true, value, Array.Empty<Error>());
    public static Result<T> Ok(T value) => Success(value);
    public static new Result<T> Failure(params Error[] errors) => new(false, default, errors);
    public static new Result<T> Failure(params string[] messages) =>
        new(false, default, messages.Select(message => new Error(message)).ToArray());
}
