using System.Collections.Immutable;
using System.Reflection;

namespace WeiPoX.Paging.Test;

[TestClass]
public class TestCollection
{
    [TestMethod]
    public async Task TestRefresh()
    {
        var config = new PagingConfig(20);
        var pagingCollection = new PagingCollection<int?, string>(config, new TestPagingSource());
        var loadState = new LoadStates(LoadState.Loading, LoadState.NotLoading.InComplete);
        using var subscribe = pagingCollection.LoadStates.Subscribe(state => loadState = state);
        Assert.IsTrue(pagingCollection.Count == 0);
        Assert.IsTrue(loadState.Refresh == LoadState.Loading);
        Assert.IsTrue(loadState.Append.IsInComplete());
        await Task.Delay(1000);
        Assert.IsTrue(pagingCollection.Count == config.InitialLoadSize);
        Assert.IsTrue(loadState.Refresh.IsComplete());
        Assert.IsTrue(loadState.Append.IsInComplete());
        pagingCollection.Refresh();
        Assert.IsTrue(loadState.Refresh == LoadState.Loading);
        Assert.IsTrue(loadState.Append.IsInComplete());
        await Task.Delay(1000);
        Assert.IsTrue(pagingCollection.Count == config.InitialLoadSize);
        Assert.IsTrue(loadState.Refresh.IsComplete());
        Assert.IsTrue(loadState.Append.IsInComplete());
        _ = pagingCollection[0];
        Assert.IsTrue(loadState.Refresh.IsComplete());
        Assert.IsTrue(loadState.Append.IsInComplete());
        _ = pagingCollection[Convert.ToInt32(config.InitialLoadSize) - 1];
        Assert.IsTrue(loadState.Refresh.IsComplete());
        Assert.IsTrue(loadState.Append == LoadState.Loading);
        await Task.Delay(1000);
        Assert.IsTrue(pagingCollection.Count == config.InitialLoadSize + config.PageSize);
        Assert.IsTrue(loadState.Refresh.IsComplete());
        Assert.IsTrue(loadState.Append.IsInComplete());
        _ = pagingCollection[Convert.ToInt32(config.InitialLoadSize) + Convert.ToInt32(config.PageSize) - 1];
        Assert.IsTrue(loadState.Refresh.IsComplete());
        Assert.IsTrue(loadState.Append == LoadState.Loading);
        await Task.Delay(1000);
        Assert.IsTrue(pagingCollection.Count == config.InitialLoadSize + config.PageSize * 2);
        Assert.IsTrue(loadState.Refresh.IsComplete());
        Assert.IsTrue(loadState.Append.IsComplete());
        _ = pagingCollection[Convert.ToInt32(config.InitialLoadSize) + Convert.ToInt32(config.PageSize) * 2 - 1];
        Assert.IsTrue(loadState.Refresh.IsComplete());
        Assert.IsTrue(loadState.Append.IsComplete());
    }
}

class TestPagingSource : IPagingSource<int?, string>
{
    public async Task<ILoadResult> Load(ILoadParams<int?> loadParams, CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);
        return loadParams switch
        {
            LoadParams.Append<int?> append => new LoadResult.Page<int?, string>(
                Enumerable.Range(0, Convert.ToInt32(append.LoadSize)).Select(i => (i * append.Key ?? 1).ToString()).ToImmutableList(), append.Key == 3 ? null : append.Key + 1),
            LoadParams.Refresh<int?> refresh => new LoadResult.Page<int?, string>(
                Enumerable.Range(0, Convert.ToInt32(refresh.LoadSize)).Select(i => i.ToString()).ToImmutableList(), 2),
            _ => throw new ArgumentOutOfRangeException(nameof(loadParams))
        };
    }
}