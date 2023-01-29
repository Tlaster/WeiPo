using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class RendererTest
{
    [TestMethod]
    public void TestRenderer()
    {
        var owner = new TestBuildOwner();
        var box = Box(Text("hello"));
        var renderer = new TestWidgetBuilder(owner);
        var result = renderer.BuildIfNeeded(null, box, null);
        Assert.AreEqual(1, result.UpdateCount);
    }

    [TestMethod]
    public void TestRendererUpdateReference()
    {
        var owner = new TestBuildOwner();
        var control = new TestControl
        {
            UpdateCount = 1
        };
        var renderer = new TestWidgetBuilder(owner);
        var result = renderer.BuildIfNeeded(
            Box(Text("hello")),
            Box(Text("world")),
            control
        );
        Assert.AreSame(control, result);
    }

    [TestMethod]
    public void TestRendererUpdate()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = renderer.BuildIfNeeded(
            Box(Text("hello")),
            Box(Text("world")),
            new TestControl { UpdateCount = 1 }
        );
        Assert.AreEqual(2, result.UpdateCount);
    }

    [TestMethod]
    public void TestRendererNotUpdate()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = renderer.BuildIfNeeded(
            Box(Text("hello")),
            Box(Text("hello")),
            new TestControl { UpdateCount = 1 }
        );
        Assert.AreEqual(1, result.UpdateCount);
    }

    [TestMethod]
    public void TestChildrenUpdateReference()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var child = new TestControl { UpdateCount = 1 };
        var result = renderer.BuildIfNeeded(
            Box(Text("hello")),
            Box(Text("world")),
            new TestPanel
            {
                UpdateCount = 1,
                Children =
                {
                    child
                }
            }
        );
        Assert.IsInstanceOfType(result, typeof(TestPanel));
        Assert.AreSame(child, ((TestPanel)result).Children[0]);
    }

    [TestMethod]
    public void TestChildrenUpdate()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = renderer.BuildIfNeeded(
            Box(Text("hello")),
            Box(Text("world")),
            new TestPanel
            {
                UpdateCount = 1,
                Children =
                {
                    new TestControl { UpdateCount = 1 }
                }
            }
        );
        Assert.IsInstanceOfType(result, typeof(TestPanel));
        Assert.AreEqual(2, ((TestPanel)result).Children[0].UpdateCount);
    }

    [TestMethod]
    public void TestChildAdd()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = renderer.BuildIfNeeded(
            Box(Text("hello")),
            Box(Text("hello"), Text("world")),
            new TestPanel
            {
                UpdateCount = 1,
                Children =
                {
                    new TestControl { UpdateCount = 1 }
                }
            }
        );
        Assert.AreEqual(2, ((TestPanel)result).Children.Count);
        Assert.AreEqual(1, ((TestPanel)result).Children[0].UpdateCount);
        Assert.AreEqual(1, ((TestPanel)result).Children[1].UpdateCount);
    }

    [TestMethod]
    public void TestChildRemove()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = renderer.BuildIfNeeded(
            Box(Text("hello"), Text("world")),
            Box(Text("hello")),
            new TestPanel
            {
                UpdateCount = 1,
                Children =
                {
                    new TestControl { UpdateCount = 1 },
                    new TestControl { UpdateCount = 1 }
                }
            }
        );
        Assert.AreEqual(1, ((TestPanel)result).Children.Count);
        Assert.AreEqual(1, ((TestPanel)result).Children[0].UpdateCount);
    }

    [TestMethod]
    public void TestChildAddUpdateReference()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var child = new TestControl { UpdateCount = 1 };
        var result = renderer.BuildIfNeeded(
            Box(Text("hello1")),
            Box(Text("hello2"), Text("world")),
            new TestPanel
            {
                UpdateCount = 1,
                Children =
                {
                    child
                }
            }
        );
        Assert.AreSame(child, ((TestPanel)result).Children[0]);
        Assert.AreEqual(2, ((TestPanel)result).Children[0].UpdateCount);
    }
}

internal class TestControl
{
    public int UpdateCount { get; set; }
}

internal class TestPanel : TestControl
{
    public List<TestControl> Children { get; } = new();
}

internal class TestBuildOwner : IBuildOwner
{
    public List<Widget> RebuiltWidgets { get; } = new();

    public bool NeedsBuild => RebuiltWidgets.Count > 0;


    public void MarkNeedsBuild(Widget widget)
    {
        RebuiltWidgets.Add(widget);
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return RebuiltWidgets.Contains(widget);
    }

    public void CleanUp()
    {
        RebuiltWidgets.Clear();
    }
}

internal class TestWidgetBuilder : WidgetBuilder<TestControl>
{
    public TestWidgetBuilder(IBuildOwner owner) : base(owner)
    {
    }

    protected override IRenderer<TestControl> GetRenderer(Type widgetType)
    {
        if (typeof(IPanelWidget).IsAssignableFrom(widgetType))
        {
            return new TestPanelRenderer();
        }

        return new TestRenderer();
    }

    protected override bool IsPanel(TestControl value)
    {
        return value is TestPanel;
    }

    protected override TestControl? GetChildAt(TestControl control, int index)
    {
        return control is TestPanel panel ? panel.Children.ElementAtOrDefault(index) : null;
    }
}

internal class TestPanelRenderer : IRenderer<TestControl>
{
    public TestControl Create()
    {
        return new TestPanel();
    }

    public void Update(TestControl control, MappingWidget widget)
    {
        control.UpdateCount++;
    }

    public void AddChild(TestControl control, TestControl childControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children.Add(childControl);
        }
    }

    public void RemoveChild(TestControl control, TestControl childControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children.Remove(childControl);
        }
    }

    public void ReplaceChild(TestControl control, int index, TestControl newChildControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children[index] = newChildControl;
        }
    }
}

internal class TestRenderer : IRenderer<TestControl>
{
    public TestControl Create()
    {
        return new TestControl();
    }

    public void Update(TestControl control, MappingWidget widget)
    {
        control.UpdateCount++;
    }

    public void AddChild(TestControl control, TestControl childControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children.Add(childControl);
        }
    }

    public void RemoveChild(TestControl control, TestControl childControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children.Remove(childControl);
        }
    }

    public void ReplaceChild(TestControl control, int index, TestControl newChildControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children[index] = newChildControl;
        }
    }
}