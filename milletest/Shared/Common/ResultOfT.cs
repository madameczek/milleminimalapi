#pragma warning disable CS8618
namespace milletest.Shared.Common;

public class Result<T> : Result
{
    private T Data { get; }

    private Result(T data)
    {
        Data = data;
    }

    private Result(Exception e) : base(e)
    { }

    public TResult Match<TResult>(Func<T, TResult> success, Func<Exception, TResult> error)
        => IsSuccessful ? success(Data) : error(Exception);

    public Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> success, Func<Exception, TResult> error)
        => IsSuccessful ? success(Data) : Task.FromResult(error(Exception));

    public void Match(Action<T> success, Action error)
    {
        if (IsSuccessful) success(Data);
        else error();
    }

    public void Match(Action<T> success, Action<Exception> error)
    {
        if (IsSuccessful) success(Data);
        else error(Exception);
    }

    public Task MatchAsync(Func<T, Task> success, Func<Exception, Task> error)
        => IsSuccessful ? success(Data) : error(Exception);

    public static Result<T> Success(T data) => new(data);
    public new static Result<T> Error(Exception error) => new(error);
}