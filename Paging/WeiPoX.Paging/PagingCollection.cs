using System.Collections.ObjectModel;
using DynamicData;

namespace WeiPoX.Paging;

public class PagingCollection<TKey, TValue> : ObservableCollection<TValue>
{
    private readonly PagingConfig _config;
    private readonly MutableLoadStateCollection _loadStateCollection = new();
    private readonly SemaphoreSlim _mutex = new(1);
    private readonly IPagingSource<TKey, TValue> _source;
    private LoadKeyHolder<TKey> _loadKeyHolder = new(default, default);

    public PagingCollection(PagingConfig config, IPagingSource<TKey, TValue> source)
    {
        _config = config;
        _source = source;
        Refresh();
    }

    public new TValue this[int index]
    {
        get
        {
            LoadIfNeeded(index);
            return base[index];
        }
    }

    public IObservable<LoadStates> LoadStates => _loadStateCollection.LoadStates;

    private void LoadIfNeeded(int index)
    {
        if (index >= Count - _config.PrefetchDistance && _loadStateCollection.GetAppend().IsInComplete() && _loadStateCollection.GetRefresh().IsComplete())
        {
            _ = LoadAsync(LoadType.Append, _config.PageSize, new CancellationToken(false));
        }
    }

    public void Refresh()
    {
        _ = LoadAsync(LoadType.Refresh, _config.InitialLoadSize, new CancellationToken(false));
    }

    private async Task LoadAsync(LoadType loadType, uint count, CancellationToken cancellationToken)
    {
        await _mutex.WaitAsync(cancellationToken);
        if (!cancellationToken.IsCancellationRequested)
        {
            _loadStateCollection.Set(loadType, LoadState.Loading);
        }

        ILoadParams<TKey> loadParams = loadType switch
        {
            LoadType.Refresh => new LoadParams.Refresh<TKey>(count),
            LoadType.Append => new LoadParams.Append<TKey>(count, _loadKeyHolder.Append),
            _ => throw new ArgumentOutOfRangeException(nameof(loadType), loadType, null)
        };
        var result = await _source.Load(loadParams, cancellationToken);
        switch (result)
        {
            case LoadResult.Error<TKey, TValue> error:
                _loadStateCollection.Set(loadType, new LoadState.Error(error.Exception));
                break;
            case LoadResult.Page<TKey, TValue> page:
                _loadKeyHolder = _loadKeyHolder with { Append = page.NextKey };
                if (loadType == LoadType.Refresh)
                {
                    _loadStateCollection.Set(LoadType.Refresh, LoadState.NotLoading.Complete);
                    Clear();
                }

                _loadStateCollection.Set(LoadType.Append,
                    page.NextKey == null ? LoadState.NotLoading.Complete : LoadState.NotLoading.InComplete);
                this.AddRange(page.Data);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result));
        }

        _mutex.Release();
    }
}