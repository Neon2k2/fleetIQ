namespace AuthenticationService.Application.Common;

public class Result
{
    public bool Succeeded { get; protected set; }
    public string[] Errors { get; protected set; }

    protected Result(bool succeeded, string[] errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public static Result Success()
        => new Result(true, Array.Empty<string>());

    public static Result Failure(params string[] errors)
        => new Result(false, errors);
}

public class Result<T> : Result
{
    public T Data { get; private set; }

    protected Result(T data, bool succeeded, string[] errors)
        : base(succeeded, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data)
        => new Result<T>(data, true, Array.Empty<string>());

    public new static Result<T> Failure(params string[] errors)
        => new Result<T>(default, false, errors);
}
