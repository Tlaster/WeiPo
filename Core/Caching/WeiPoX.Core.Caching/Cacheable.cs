using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WeiPoX.Core.Caching;

public class Cacheable<TValue>
{
    private readonly ReplaySubject<int> _invalidationSubject = new();

    public Cacheable(Func<IObservable<TValue>> source, Func<Task<LoadResult>> remoteMediator)
    {
        Data = source.Invoke().Select(it => new CacheState.Success<TValue>(it) as CacheState)
            .StartWith(new CacheState.Empty());
        RefreshState = _invalidationSubject
            .SelectMany(Observable.FromAsync(remoteMediator.Invoke))
            .Select<LoadResult, LoadState>(it => it switch
            {
                LoadResult.Error error => new LoadState.Error(error.Exception),
                LoadResult.Success => new LoadState.Success(),
                _ => throw new ArgumentOutOfRangeException(nameof(it))
            })
            .StartWith(new LoadState.Loading());
    }

    public IObservable<CacheState> Data { get; }

    public IObservable<LoadState> RefreshState { get; }

    public void Refresh()
    {
        _invalidationSubject.OnNext(0);
    }
}

public abstract record LoadResult
{
    public record Success : LoadResult;

    public record Error(Exception Exception) : LoadResult;
}

public abstract record LoadState
{
    public record Loading : LoadState;

    public record Success : LoadState;

    public record Error(Exception Exception) : LoadState;
}

public abstract record CacheState
{
    public record Empty : CacheState;

    public record Success<T>(T Value) : CacheState;
}