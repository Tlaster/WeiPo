using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Test.WidgetTest;

[TestClass]
public class RowTest
{
    [TestMethod]
    public void TestCreateRow()
    {
        var row = new Row();
        var renderer = new WidgetBuilder(new TestOwner());
        var control = renderer.BuildIfNeeded(null, row, null);
        Assert.IsInstanceOfType(control, typeof(StackPanel));
        Assert.AreEqual(Orientation.Horizontal, ((StackPanel)control).Orientation);
    }
}

internal class TestOwner : IBuildOwner
{
    public void MarkNeedsBuild(Widget widget)
    {
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return false;
    }

    public void CleanUp()
    {
    }
}