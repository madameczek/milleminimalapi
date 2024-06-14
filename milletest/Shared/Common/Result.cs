#pragma warning disable CS8618

namespace milletest.Shared.Common;

public class Result
{
    protected bool IsSuccessful { get; }
    protected Exception Exception { get; }

    private protected Result()
    {
        IsSuccessful = true;
    }

    private protected Result(Exception error)
    {
        IsSuccessful = false;
        Exception = error;
    }

    public TResult Match<TResult>(Func<TResult> success, Func<Exception, TResult> error)
        => IsSuccessful ? success() : error(Exception);

    public void Match(Action success, Action error)
    {
        if (IsSuccessful) success();
        else error();
    }

    public void Match(Action success, Action<Exception> error)
    {
        if (IsSuccessful) success();
        else error(Exception);
    }

    public static Result Success() => new();
    public static Result Error(Exception error) => new(error);
}