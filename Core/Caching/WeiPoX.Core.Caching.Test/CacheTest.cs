using System.Reactive.Subjects;

namespace WeiPoX.Core.Caching.Test;

[TestClass]
public class CacheTest
{
    [TestMethod]
    public async Task TestRemoteSuccess()
    {
        var source = new Subject<string>();
        var cacheable = new Cacheable<string>(() => source, async () =>
        {
            await Task.Delay(1000);
            return new LoadResult.Success();
        });
        var dataStateList = new List<CacheState>();
        using var _ = cacheable.Data.Subscribe(x => dataStateList.Add(x));
        var loadStateList = new List<LoadState>();
        using var __ = cacheable.RefreshState.Subscribe(x => loadStateList.Add(x));
        Assert.AreEqual(1, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[0], typeof(CacheState.Empty));
        Assert.AreEqual(1, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[0], typeof(LoadState.Loading));
        cacheable.Refresh();
        await Task.Delay(500);
        Assert.AreEqual(1, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[0], typeof(CacheState.Empty));
        Assert.AreEqual(1, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[0], typeof(LoadState.Loading));
        source.OnNext("1");
        await Task.Delay(1000);
        Assert.AreEqual(2, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[1], typeof(CacheState.Success<string>));
        Assert.AreEqual("1", (dataStateList[1] as CacheState.Success<string>)?.Value);
        Assert.AreEqual(2, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[1], typeof(LoadState.Success));
    }

    [TestMethod]
    public async Task TestRemoteFailure()
    {
        var source = new Subject<string>();
        var cacheable = new Cacheable<string>(() => source, async () =>
        {
            await Task.Delay(1000);
            return new LoadResult.Error(new Exception());
        });
        var dataStateList = new List<CacheState>();
        using var _ = cacheable.Data.Subscribe(x => dataStateList.Add(x));
        var loadStateList = new List<LoadState>();
        using var __ = cacheable.RefreshState.Subscribe(x => loadStateList.Add(x));
        Assert.AreEqual(1, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[0], typeof(CacheState.Empty));
        Assert.AreEqual(1, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[0], typeof(LoadState.Loading));
        cacheable.Refresh();
        await Task.Delay(1000);
        Assert.AreEqual(1, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[0], typeof(CacheState.Empty));
        Assert.AreEqual(1, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[0], typeof(LoadState.Loading));
        await Task.Delay(500);
        Assert.AreEqual(1, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[0], typeof(CacheState.Empty));
        Assert.AreEqual(2, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[1], typeof(LoadState.Error));
    }

    [TestMethod]
    public async Task TestLocalSuccess()
    {
        var source = new Subject<string>();
        var cacheable = new Cacheable<string>(() => source, async () =>
        {
            await Task.Delay(1000);
            return new LoadResult.Success();
        });
        var dataStateList = new List<CacheState>();
        using var _ = cacheable.Data.Subscribe(x => dataStateList.Add(x));
        var loadStateList = new List<LoadState>();
        using var __ = cacheable.RefreshState.Subscribe(x => loadStateList.Add(x));
        Assert.AreEqual(1, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[0], typeof(CacheState.Empty));
        Assert.AreEqual(1, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[0], typeof(LoadState.Loading));
        source.OnNext("1");
        Assert.AreEqual(2, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[1], typeof(CacheState.Success<string>));
        Assert.AreEqual("1", (dataStateList[1] as CacheState.Success<string>)?.Value);
        Assert.AreEqual(1, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[0], typeof(LoadState.Loading));
        cacheable.Refresh();
        await Task.Delay(2000);
        Assert.AreEqual(2, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[1], typeof(CacheState.Success<string>));
        Assert.AreEqual("1", (dataStateList[1] as CacheState.Success<string>)?.Value);
        Assert.AreEqual(2, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[1], typeof(LoadState.Success));
    }

    [TestMethod]
    public async Task TestLocalFailure()
    {
        var source = new Subject<string>();
        var cacheable = new Cacheable<string>(() => source, async () =>
        {
            await Task.Delay(1000);
            return new LoadResult.Error(new Exception());
        });
        var dataStateList = new List<CacheState>();
        using var _ = cacheable.Data.Subscribe(x => dataStateList.Add(x));
        var loadStateList = new List<LoadState>();
        using var __ = cacheable.RefreshState.Subscribe(x => loadStateList.Add(x));
        Assert.AreEqual(1, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[0], typeof(CacheState.Empty));
        Assert.AreEqual(1, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[0], typeof(LoadState.Loading));
        source.OnNext("1");
        Assert.AreEqual(2, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[1], typeof(CacheState.Success<string>));
        Assert.AreEqual("1", (dataStateList[1] as CacheState.Success<string>)?.Value);
        Assert.AreEqual(1, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[0], typeof(LoadState.Loading));
        cacheable.Refresh();
        await Task.Delay(2000);
        Assert.AreEqual(2, dataStateList.Count);
        Assert.IsInstanceOfType(dataStateList[1], typeof(CacheState.Success<string>));
        Assert.AreEqual("1", (dataStateList[1] as CacheState.Success<string>)?.Value);
        Assert.AreEqual(2, loadStateList.Count);
        Assert.IsInstanceOfType(loadStateList[1], typeof(LoadState.Error));
    }
}