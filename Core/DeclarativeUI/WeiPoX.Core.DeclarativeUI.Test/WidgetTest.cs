using WeiPoX.Core.DeclarativeUI.Testing;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class WidgetTest
{
    [TestMethod]
    public void TestWidget()
    {
        var box = new Box
        {
            new Text("Hello"), 
            new Text("World")
        };
        Assert.IsInstanceOfType(box, typeof(Box));
        Assert.IsInstanceOfType(box.Children[0], typeof(Text));
        Assert.AreEqual("Hello", (box.Children[0] as Text)?.Content);
        Assert.IsInstanceOfType(box.Children[1], typeof(Text));
        Assert.AreEqual("World", (box.Children[1] as Text)?.Content);
    }

    [TestMethod]
    public async Task TestWidgetWithNestedStatefulWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new TestStatefulWithNestedStatefulWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = await builder.BuildIfNeededAsync(null, widget, null);
        Assert.IsInstanceOfType(widget.State.CachedBuild, typeof(Box));
        var box = widget.State.CachedBuild as Box;
        Assert.IsNotNull(box);
        Assert.IsInstanceOfType(box.Children[0], typeof(Button));
        var button = box.Children[0] as Button;
        Assert.IsNotNull(button);
        Assert.IsInstanceOfType(box.Children[1], typeof(TestNestedStatefulWidget));
        var nestedWidget = box.Children[1] as TestNestedStatefulWidget;
        Assert.IsNotNull(nestedWidget);
        Assert.IsInstanceOfType(nestedWidget.State.CachedBuild, typeof(Box));
        var nestedBox = nestedWidget.State.CachedBuild as Box;
        Assert.IsNotNull(nestedBox);
        Assert.IsInstanceOfType(nestedBox.Children[0], typeof(Button));
        Assert.IsInstanceOfType(nestedBox.Children[1], typeof(Text));
        var text = nestedBox.Children[1] as Text;
        Assert.IsNotNull(text);
        Assert.AreEqual("0", text.Content);

        button.OnClick.Invoke();

        result = await builder.BuildIfNeededAsync(widget, widget, result);
        owner.CleanUp();

        Assert.IsInstanceOfType(widget.State.CachedBuild, typeof(Box));
        box = widget.State.CachedBuild as Box;
        Assert.IsNotNull(box);
        Assert.IsInstanceOfType(box.Children[0], typeof(Button));
        button = box.Children[0] as Button;
        Assert.IsNotNull(button);
        Assert.IsInstanceOfType(box.Children[1], typeof(TestNestedStatefulWidget));
        nestedWidget = box.Children[1] as TestNestedStatefulWidget;
        Assert.IsNotNull(nestedWidget);
        Assert.IsInstanceOfType(nestedWidget.State.CachedBuild, typeof(Box));
        nestedBox = nestedWidget.State.CachedBuild as Box;
        Assert.IsNotNull(nestedBox);
        Assert.IsInstanceOfType(nestedBox.Children[0], typeof(Button));
        Assert.IsInstanceOfType(nestedBox.Children[1], typeof(Text));
        text = nestedBox.Children[1] as Text;
        Assert.IsNotNull(text);
        Assert.AreEqual("0", text.Content);
    }

    [TestMethod]
    public async Task TestWidgetWithParameterNestedStatefulWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new TestStatefulWithParameterNestedStatefulWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = await builder.BuildIfNeededAsync(null, widget, null);
        Assert.IsInstanceOfType(widget.State.CachedBuild, typeof(Box));
        var box = widget.State.CachedBuild as Box;
        Assert.IsNotNull(box);
        Assert.IsInstanceOfType(box.Children[0], typeof(Button));
        var button = box.Children[0] as Button;
        Assert.IsNotNull(button);
        Assert.IsInstanceOfType(box.Children[1], typeof(TestParameterNestedStatefulWidget));
        var nestedWidget = box.Children[1] as TestParameterNestedStatefulWidget;
        Assert.IsNotNull(nestedWidget);
        Assert.IsInstanceOfType(nestedWidget.State.CachedBuild, typeof(Box));
        var nestedBox = nestedWidget.State.CachedBuild as Box;
        Assert.IsNotNull(nestedBox);
        Assert.IsInstanceOfType(nestedBox.Children[0], typeof(Button));
        Assert.IsInstanceOfType(nestedBox.Children[1], typeof(Text));
        var text = nestedBox.Children[1] as Text;
        Assert.IsNotNull(text);
        Assert.AreEqual("0", text.Content);
        Assert.IsInstanceOfType(nestedBox.Children[2], typeof(Text));
        var text2 = nestedBox.Children[2] as Text;
        Assert.IsNotNull(text2);
        Assert.AreEqual("0", text2.Content);

        button.OnClick.Invoke();

        result = await builder.BuildIfNeededAsync(widget, widget, result);
        owner.CleanUp();

        Assert.IsInstanceOfType(widget.State.CachedBuild, typeof(Box));
        box = widget.State.CachedBuild as Box;
        Assert.IsNotNull(box);
        Assert.IsInstanceOfType(box.Children[0], typeof(Button));
        button = box.Children[0] as Button;
        Assert.IsNotNull(button);
        Assert.IsInstanceOfType(box.Children[1], typeof(TestParameterNestedStatefulWidget));
        nestedWidget = box.Children[1] as TestParameterNestedStatefulWidget;
        Assert.IsNotNull(nestedWidget);
        Assert.IsInstanceOfType(nestedWidget.State.CachedBuild, typeof(Box));
        nestedBox = nestedWidget.State.CachedBuild as Box;
        Assert.IsNotNull(nestedBox);
        Assert.IsInstanceOfType(nestedBox.Children[0], typeof(Button));
        var nestedButton = nestedBox.Children[0] as Button;
        Assert.IsNotNull(nestedButton);
        Assert.IsInstanceOfType(nestedBox.Children[1], typeof(Text));
        text = nestedBox.Children[1] as Text;
        Assert.IsNotNull(text);
        Assert.AreEqual("0", text.Content);
        Assert.IsInstanceOfType(nestedBox.Children[2], typeof(Text));
        text2 = nestedBox.Children[2] as Text;
        Assert.IsNotNull(text2);
        Assert.AreEqual("1", text2.Content);

        nestedButton.OnClick.Invoke();
        result = await builder.BuildIfNeededAsync(widget, widget, result);
        owner.CleanUp();

        Assert.IsInstanceOfType(widget.State.CachedBuild, typeof(Box));
        box = widget.State.CachedBuild as Box;
        Assert.IsNotNull(box);
        Assert.IsInstanceOfType(box.Children[0], typeof(Button));
        button = box.Children[0] as Button;
        Assert.IsNotNull(button);
        Assert.IsInstanceOfType(box.Children[1], typeof(TestParameterNestedStatefulWidget));
        nestedWidget = box.Children[1] as TestParameterNestedStatefulWidget;
        Assert.IsNotNull(nestedWidget);
        Assert.IsInstanceOfType(nestedWidget.State.CachedBuild, typeof(Box));
        nestedBox = nestedWidget.State.CachedBuild as Box;
        Assert.IsNotNull(nestedBox);
        Assert.IsInstanceOfType(nestedBox.Children[0], typeof(Button));
        Assert.IsInstanceOfType(nestedBox.Children[1], typeof(Text));
        text = nestedBox.Children[1] as Text;
        Assert.IsNotNull(text);
        Assert.AreEqual("1", text.Content);
        Assert.IsInstanceOfType(nestedBox.Children[2], typeof(Text));
        text2 = nestedBox.Children[2] as Text;
        Assert.IsNotNull(text2);
        Assert.AreEqual("1", text2.Content);
    }

    private record TestStatefulWithNestedStatefulWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return new Box
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => setValue(value + 1)
                },
                new TestNestedStatefulWidget()
            };
        }
    }

    private record TestNestedStatefulWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return new Box
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => setValue(value + 1)
                },
                new Text(value.ToString())
            };
        }
    }

    private record TestStatefulWithParameterNestedStatefulWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return new Box
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => setValue(value + 1)
                },
                new TestParameterNestedStatefulWidget(value)
            };
        }
    }

    private record TestParameterNestedStatefulWidget(int Value) : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return new Box
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => setValue(value + 1)
                },
                new Text(value.ToString()),
                new Text(Value.ToString())
            };
        }
    }
    record TestTreeChangedWidget : StatefulWidget
    {

        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return new Box
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => setValue(value + 1)
                },
                new Text(value.ToString()),
                value == 1 ? new Text("1") : new Box(),
            };
        }
    }
    
    [TestMethod]
    public void TestTreeChanged()
    {
        var test = new TestTreeChangedWidget().Test();
        var widget = test.GetWidget<TestTreeChangedWidget>();
        var button = widget.GetChild<Box>()?.FindChildAtIndex<Button>(0);
        Assert.IsNotNull(button);
        var box = widget.GetChild<Box>()?.FindChildAtIndex<Box>(2);
        Assert.IsNotNull(box);
        button.OnClick.Invoke();
        var text = widget.GetChild<Box>()?.FindChildAtIndex<Text>(2);
        Assert.IsNotNull(text);
        Assert.AreEqual("1", text.Content);
        button = widget.GetChild<Box>()?.FindChildAtIndex<Button>(0);
        Assert.IsNotNull(button);
        button.OnClick.Invoke();
        box = widget.GetChild<Box>()?.FindChildAtIndex<Box>(2);
        Assert.IsNotNull(box);
    }
    
    record TestMultiPassWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            var (value2, setValue2) = UseState(0);
            UseEffect(() => { setValue2(value); }, value);
            return new Box
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => setValue(value + 1)
                },
                new Text(value.ToString()),
                value == 1 ? new Text("1") : new Box(),
            };
        }
    }
    
    [TestMethod]
    public void TestMultiPass()
    {
        var test = new TestMultiPassWidget().Test();
        var widget = test.GetWidget<TestMultiPassWidget>();
        var button = widget.GetChild<Box>()?.FindChildAtIndex<Button>(0);
        Assert.IsNotNull(button);
        var box = widget.GetChild<Box>()?.FindChildAtIndex<Box>(2);
        Assert.IsNotNull(box);
        button.OnClick.Invoke();
        var text = widget.GetChild<Box>()?.FindChildAtIndex<Text>(2);
        Assert.IsNotNull(text);
        Assert.AreEqual("1", text.Content);
        button = widget.GetChild<Box>()?.FindChildAtIndex<Button>(0);
        Assert.IsNotNull(button);
        button.OnClick.Invoke();
        box = widget.GetChild<Box>()?.FindChildAtIndex<Box>(2);
        Assert.IsNotNull(box);
    }
}