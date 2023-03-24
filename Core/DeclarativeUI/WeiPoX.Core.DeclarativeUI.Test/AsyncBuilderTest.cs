using WeiPoX.Core.DeclarativeUI.Testing;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class AsyncBuilderTest
{
    [TestMethod]
    public async Task TestAsyncBuilder()
    {
        var owner = new TestBuildOwner();
        var box = new Box
        {
            new Text("hello")
        };
        var renderer = new TestWidgetBuilder(owner);
        var result = await renderer.BuildIfNeededAsync(null, box, null);
        Assert.AreEqual(1, result.UpdateCount);
    }
}