namespace CommuniGate.Results;

public record Result : IResult
{
    public bool IsSuccess { get; }
    public Exception? Exception { get; }

    public Result()
    {
        IsSuccess = true;
    }

    public Result(Exception exception)
    {
        Exception = exception;
        IsSuccess = false;
    }

    public void Match(Action success, Action<Exception> failure)
    {
        if (IsSuccess)
            success();
        else failure(Exception!);
    }

    public void IfSuccess(Action action)
    {
        if (IsSuccess) action();
    }

    public void IfFailure(Action<Exception> action)
    {
        if (!IsSuccess) action(Exception!);
    }
}

public record Result<TResult> : Result, IResult<TResult>
{
    public Result(TResult result)
    {
        Value = result;
    }

    public Result(Exception exception) : base(exception)
    {
    }

    public TResult? Value { get; }

    public IResult<TResult> IfSuccess(Func<TResult, TResult> func)
    {
        return IsSuccess ? new Result<TResult>(func(Value!)) : this;
    }

    public void IfSuccess(Action<TResult> action)
    {
        if (IsSuccess) action(Value!);
    }

    public IResult<TResult> IfFailure(Func<Exception, TResult> func)
    {
        return !IsSuccess ? new Result<TResult>(func(Exception!)) : this;
    }

    public IResult<TResult> Match(Func<TResult, TResult> success, Func<Exception, TResult> failure)
    {
        return IsSuccess ? new Result<TResult>(success(Value!)) : new Result<TResult>(failure(Exception!));
    }

    public IResult<TMapped> Map<TMapped>(Func<TResult, TMapped> success)
    {
        return IsSuccess ? new Result<TMapped>(success(Value!)) : new Result<TMapped>(Exception!);
    }

    public IResult<TMapped> Map<TMapped>(Func<TResult, TMapped> success, Func<Exception, TMapped> failure)
    {
        return IsSuccess ? new Result<TMapped>(success(Value!)) : new Result<TMapped>(failure(Exception!));
    }
}