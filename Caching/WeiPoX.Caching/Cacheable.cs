using System.Reactive;
using System.Reactive.Linq;
using CSharpFunctionalExtensions;

namespace WeiPoX.Caching;

public class Cacheable<TKey, TValue>
{
    private readonly IRemoteMediator _remoteMediator;
    private readonly ICachingSource<TKey, TValue> _source;

    public Cacheable(ICachingSource<TKey, TValue> source, IRemoteMediator remoteMediator)
    {
        _source = source;
        _remoteMediator = remoteMediator;
    }

    public IObservable<LoadState> Get(TKey key)
    {
        var remoteObservable = Observable.FromAsync(() => _remoteMediator.RefreshAsync())
            .Select<Result, LoadState>(it =>
                it.IsSuccess ? new LoadState.Success<Unit>(Unit.Default) : new LoadState.Failure(it.Error))
            .StartWith(new LoadState.Loading());
        var localObservable = _source.Get(key)
            .Select<TValue, LoadState>(it => new LoadState.Success<TValue>(it))
            .StartWith(new LoadState.NoData());
        return remoteObservable.CombineLatest(localObservable, (remote, local) =>
        {
            return local switch
            {
                LoadState.Failure failure => failure,
                LoadState.Loading loading => loading,
                LoadState.NoData => remote is not LoadState.Failure ? new LoadState.Loading() : remote,
                LoadState.Success<TValue> success => success,
                _ => throw new ArgumentOutOfRangeException(nameof(local))
            };
        }).DistinctUntilChanged();
    }
}

public record LoadState
{
    public record Loading : LoadState;

    public record NoData : LoadState;

    public record Success<T>(T Value) : LoadState;

    public record Failure(string Exception) : LoadState;
}