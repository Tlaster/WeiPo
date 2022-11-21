using System.Reactive.Subjects;
using CSharpFunctionalExtensions;

namespace WeiPoX.Core.Caching.Test;

[TestClass]
public class CacheTest
{
    [TestMethod]
    public async Task TestRemoteSuccess()
    {
        var source = new TestCachingSource();
        var mediator = new TestSuccessRemoteMediator();
        var cacheable = new Cacheable<int, string>(source, mediator);
        var observable = cacheable.Get(1);
        var stateList = new List<LoadState>();
        using var _ = observable.Subscribe(x => stateList.Add(x));
        Assert.AreEqual(1, stateList.Count);
        Assert.IsInstanceOfType(stateList[0], typeof(LoadState.Loading));
        await Task.Delay(TimeSpan.FromSeconds(3));
        Assert.AreEqual(1, stateList.Count);
        Assert.IsInstanceOfType(stateList[0], typeof(LoadState.Loading));
        source.Emit("1");
        Assert.AreEqual(2, stateList.Count);
        Assert.IsInstanceOfType(stateList[1], typeof(LoadState.Success<string>));
        Assert.AreEqual("1", (stateList[1] as LoadState.Success<string>)?.Value);
    }

    [TestMethod]
    public async Task TestRemoteFailure()
    {
        var source = new TestCachingSource();
        var mediator = new TestFailureRemoteMediator();
        var cacheable = new Cacheable<int, string>(source, mediator);
        var observable = cacheable.Get(1);
        var stateList = new List<LoadState>();
        using var _ = observable.Subscribe(x => stateList.Add(x));
        Assert.AreEqual(1, stateList.Count);
        Assert.IsInstanceOfType(stateList[0], typeof(LoadState.Loading));
        await Task.Delay(TimeSpan.FromSeconds(3));
        Assert.AreEqual(2, stateList.Count);
        Assert.IsInstanceOfType(stateList[1], typeof(LoadState.Failure));
    }

    [TestMethod]
    public async Task TestRemoteFailureWithPreCache()
    {
        var source = new TestCachingSource();
        source.Emit("1");
        var mediator = new TestFailureRemoteMediator();
        var cacheable = new Cacheable<int, string>(source, mediator);
        var observable = cacheable.Get(1);
        var stateList = new List<LoadState>();
        using var _ = observable.Subscribe(x => stateList.Add(x));
        await Task.Delay(TimeSpan.FromSeconds(3));
        Assert.AreEqual(2, stateList.Count);
        Assert.IsInstanceOfType(stateList[0], typeof(LoadState.Loading));
        Assert.IsInstanceOfType(stateList[1], typeof(LoadState.Success<string>));
        Assert.AreEqual("1", (stateList[1] as LoadState.Success<string>)?.Value);
    }

    [TestMethod]
    public async Task TestRemoteSuccessWithPreCache()
    {
        var source = new TestCachingSource();
        source.Emit("1");
        var mediator = new TestSuccessRemoteMediator();
        var cacheable = new Cacheable<int, string>(source, mediator);
        var observable = cacheable.Get(1);
        var stateList = new List<LoadState>();
        using var _ = observable.Subscribe(x => stateList.Add(x));
        await Task.Delay(TimeSpan.FromSeconds(2));
        source.Emit("2");
        await Task.Delay(TimeSpan.FromSeconds(1));
        Assert.AreEqual(3, stateList.Count);
        Assert.IsInstanceOfType(stateList[0], typeof(LoadState.Loading));
        Assert.IsInstanceOfType(stateList[1], typeof(LoadState.Success<string>));
        Assert.AreEqual("1", (stateList[1] as LoadState.Success<string>)?.Value);
        Assert.IsInstanceOfType(stateList[2], typeof(LoadState.Success<string>));
        Assert.AreEqual("2", (stateList[2] as LoadState.Success<string>)?.Value);
    }
}

internal class TestCachingSource : ICachingSource<int, string>
{
    private readonly ReplaySubject<string> _data = new();

    public IObservable<string> Get(int key)
    {
        return _data;
    }

    public void Emit(string data)
    {
        _data.OnNext(data);
    }
}

internal class TestSuccessRemoteMediator : IRemoteMediator
{
    public async Task<Result> RefreshAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return Result.Success();
    }
}

internal class TestFailureRemoteMediator : IRemoteMediator
{
    public async Task<Result> RefreshAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return Result.Failure("error");
    }
}