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
    public void TestWidgetWithNestedStatefulWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new TestStatefulWithNestedStatefulWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
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

        result = builder.BuildIfNeeded(widget, widget, result);
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
    public void TestWidgetWithParameterNestedStatefulWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new TestStatefulWithParameterNestedStatefulWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
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

        result = builder.BuildIfNeeded(widget, widget, result);
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
        result = builder.BuildIfNeeded(widget, widget, result);
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
}