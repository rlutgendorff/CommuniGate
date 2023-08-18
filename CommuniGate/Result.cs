namespace CommuniGate;

public class Result : IResult
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

}

public class Result<TResult> : Result, IResult<TResult>
{
    public Result(TResult result)
    {
        Value = result;
    }

    public Result(Exception exception) : base(exception) 
    {
    }

    public TResult? Value { get; }
}