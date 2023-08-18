namespace CommuniGate;

public interface IResult
{
    bool IsSuccess { get; }
    Exception? Exception { get; }
}

public interface IResult<out TResult> : IResult
{
    TResult? Value { get; }
}