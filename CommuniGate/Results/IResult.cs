namespace CommuniGate.Results;

public interface IBaseResult
{
    bool IsSuccess { get; }
    Exception? Exception { get; }

    void Match(Action success, Action<Exception> failure);
}

public interface IResult : IBaseResult
{
    void WhenSuccess(Action action);
    void WhenFailure(Action<Exception> action);
    
}

public interface IResult<TResult> : IBaseResult
{
    TResult? Value { get; }

    IResult<TResult> WhenSuccess(Func<TResult, TResult> func);
    void WhenSuccess(Action<TResult> action);
    IResult<TResult> WhenFailure(Func<Exception, TResult> func);
    void WhenFailure(Action<Exception> action);

    IResult<TResult> Match(Func<TResult, TResult> success, Func<Exception, TResult> failure);

    IResult<TMapped> Map<TMapped>(Func<TResult, TMapped> success);
    IResult<TMapped> Map<TMapped>(Func<TResult, TMapped> success, Func<Exception, TMapped> failure);



}