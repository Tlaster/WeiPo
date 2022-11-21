using Microsoft.UI.Xaml.Controls;
using WeiPoX.Core.DeclarativeUI.WinUI3.Internal;

namespace WeiPoX.Core.DeclarativeUI.WinUI3.Test.WidgetTest;

[TestClass]
public class RowTest
{
    [TestMethod]
    public void TestCreateRow()
    {
        var row = Row();
        var renderer = new WidgetRenderer();
        var control = renderer.RenderIfNeeded(null, row, null);
        Assert.IsInstanceOfType(control, typeof(StackPanel));
        Assert.AreEqual(Orientation.Horizontal, ((StackPanel)control).Orientation);
    }
}