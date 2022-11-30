using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class WidgetTest
{
    [TestMethod]
    public void TestWidget()
    {
        var box = Box(Text("Hello"), Text("World"));
        Assert.IsInstanceOfType(box, typeof(Box));
        Assert.IsInstanceOfType(box.Children[0], typeof(Text));
        Assert.AreEqual((box.Children[0] as Text)?.Content, "Hello");
        Assert.IsInstanceOfType(box.Children[1], typeof(Text));
        Assert.AreEqual((box.Children[1] as Text)?.Content, "World");
    }
}