using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class StateTest
{
    record TestUseStateWidget: StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return Row(
                Button(
                    () => { setValue.Invoke(value + 1); },
                    Text("Click")
                ),
                Text(value.ToString())
            );
        }
    }

    record TestStatelessWidget : StatelessWidget
    {
        protected internal override Widget Content { get; } = Column(
            Text("Hello"),
            new NestedStatelessWidget()
        );
    }
    
    record NestedStatelessWidget : StatelessWidget
    {
        protected internal override Widget Content { get; } = Column(Text("World"));
    }

    [TestMethod]
    public void TestNestedWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new TestStatelessWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        Assert.IsInstanceOfType(result, typeof(TestPanel));
        var panel = (TestPanel) result;
        Assert.AreEqual(2, panel.Children.Count);
        Assert.IsInstanceOfType(panel.Children[0], typeof(TestControl));
        Assert.IsInstanceOfType(panel.Children[1], typeof(TestPanel));
        var panel2 = (TestPanel) panel.Children[1];
        Assert.AreEqual(1, panel2.Children.Count);
        Assert.IsInstanceOfType(panel2.Children[0], typeof(TestControl));
    }
    
    [TestMethod]
    public void TestStatefulState()
    {
        var owner = new TestBuildOwner();
        var widget = new TestUseStateWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        Assert.IsInstanceOfType(result, typeof(TestPanel));
        var panel = (TestPanel) result;
        Assert.AreEqual(2, panel.Children.Count);
        Assert.IsInstanceOfType(panel.Children[0], typeof(TestPanel));
        Assert.IsInstanceOfType(panel.Children[1], typeof(TestControl));
        var panel2 = (TestPanel) panel.Children[0];
        Assert.AreEqual(1, panel2.Children.Count);
        Assert.IsInstanceOfType(panel2.Children[0], typeof(TestControl));

        var cache = widget.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        var row = (Row) cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(Text));
        var button = (Button) row.Children[0];
        var text = (Text) row.Children[1];
        Assert.AreEqual(1, button.Children.Count);
        Assert.AreEqual("0", text.Content);
        Assert.IsInstanceOfType(button.Children[0], typeof(Text));
        var text2 = (Text) button.Children[0];
        Assert.AreEqual("Click", text2.Content);

        Assert.IsFalse(owner.NeedsBuild);
        button.OnClick.Invoke();
        Assert.IsTrue(owner.NeedsBuild);
        Assert.AreEqual(1, owner.RebuiltWidgets.Count);
        var newResult = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        Assert.IsFalse(owner.NeedsBuild);
        Assert.AreSame(result, newResult);
        
        var newCache = widget.CachedBuild;
        Assert.IsNotNull(newCache);
        Assert.AreNotSame(cache, newCache);
        Assert.IsInstanceOfType(newCache, typeof(Row));
        var newRow = (Row) newCache;
        Assert.AreEqual(2, newRow.Children.Count);
        Assert.IsInstanceOfType(newRow.Children[0], typeof(Button));
        Assert.IsInstanceOfType(newRow.Children[1], typeof(Text));
        var newButton = (Button) newRow.Children[0];
        var newText = (Text) newRow.Children[1];
        Assert.AreEqual(1, newButton.Children.Count);
        Assert.AreEqual("1", newText.Content);
        Assert.IsInstanceOfType(newButton.Children[0], typeof(Text));
        var newText2 = (Text) newButton.Children[0];
        Assert.AreEqual("Click", newText2.Content);
    }

    record TestDisposeWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return Row(
                Button(
                    () => { setValue.Invoke(value + 1); },
                    Text("Click")
                ),
                value == 1 ? new TestUseStateWidget() : Text(value.ToString())
            );
        }
    }

    [TestMethod]
    public void TestStatefulWidgetDispose()
    {
        var owner = new TestBuildOwner();
        var widget = new TestDisposeWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        var cache = widget.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        var row = (Row) cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(Text));
        var button = (Button) row.Children[0];
        button.OnClick.Invoke();
        result = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        cache = widget.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        row = (Row) cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(TestUseStateWidget));
        var stateWidget = (TestUseStateWidget) row.Children[1];
        Assert.IsFalse(stateWidget.Disposed);
        button = (Button) row.Children[0];
        button.OnClick.Invoke();
        result = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        cache = widget.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        row = (Row) cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(Text));
        Assert.IsTrue(stateWidget.Disposed);
    }
}