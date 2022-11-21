using WeiPoX.Core.DeclarativeUI.Internal;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class RendererTest
{
    [TestMethod]
    public void TestRenderer()
    {
        var box = Box(Text("hello"));
        var renderer = new TestWidgetRenderer();
        var result = renderer.RenderIfNeeded(null, box, null);
        Assert.AreEqual(1, result.UpdateCount);
    }

    [TestMethod]
    public void TestRendererUpdateReference()
    {
        var control = new TestControl
        {
            UpdateCount = 1
        };
        var renderer = new TestWidgetRenderer();
        var result = renderer.RenderIfNeeded(
            Box(Text("hello")),
            Box(Text("world")),
            control
        );
        Assert.AreSame(control, result);
    }

    [TestMethod]
    public void TestRendererUpdate()
    {
        var renderer = new TestWidgetRenderer();
        var result = renderer.RenderIfNeeded(
            Box(Text("hello")),
            Box(Text("world")),
            new TestControl { UpdateCount = 1 }
        );
        Assert.AreEqual(2, result.UpdateCount);
    }

    [TestMethod]
    public void TestRendererNotUpdate()
    {
        var renderer = new TestWidgetRenderer();
        var result = renderer.RenderIfNeeded(
            Box(Text("hello")),
            Box(Text("hello")),
            new TestControl { UpdateCount = 1 }
        );
        Assert.AreEqual(1, result.UpdateCount);
    }

    [TestMethod]
    public void TestChildrenUpdateReference()
    {
        var renderer = new TestWidgetRenderer();
        var child = new TestControl { UpdateCount = 1 };
        var result = renderer.RenderIfNeeded(
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
        var renderer = new TestWidgetRenderer();
        var result = renderer.RenderIfNeeded(
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
        var renderer = new TestWidgetRenderer();
        var result = renderer.RenderIfNeeded(
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
        var renderer = new TestWidgetRenderer();
        var result = renderer.RenderIfNeeded(
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
        var renderer = new TestWidgetRenderer();
        var child = new TestControl { UpdateCount = 1 };
        var result = renderer.RenderIfNeeded(
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

internal class TestWidgetRenderer : WidgetRenderer<TestControl>
{
    protected override IRenderer<TestControl> GetRenderer(Type widgetType)
    {
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

internal class TestRenderer : IRenderer<TestControl>
{
    public TestControl Create()
    {
        return new TestControl();
    }

    public void Update(TestControl control, WidgetObject widget)
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