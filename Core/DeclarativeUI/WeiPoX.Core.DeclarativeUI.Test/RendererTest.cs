using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Testing;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class RendererTest
{
    [TestMethod]
    public async Task TestRenderer()
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

    [TestMethod]
    public async Task TestRendererUpdateReference()
    {
        var owner = new TestBuildOwner();
        var control = new TestControl
        {
            UpdateCount = 1
        };
        var renderer = new TestWidgetBuilder(owner);
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello")
            },
            new Box
            { 
                new Text("world")
            },
            control
        );
        Assert.AreSame(control, result);
    }

    [TestMethod]
    public async Task TestRendererUpdate()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello"),
            },
            new Box
            {
                new Text("world"),
            },
            new TestControl { UpdateCount = 1 }
        );
        Assert.AreEqual(2, result.UpdateCount);
    }

    [TestMethod]
    public async Task TestRendererNotUpdate()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello"),
            },
            new Box
            {
                new Text("hello")
            },
            new TestControl { UpdateCount = 1 }
        );
        Assert.AreEqual(1, result.UpdateCount);
    }

    [TestMethod]
    public async Task TestChildrenUpdateReference()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var child = new TestControl { UpdateCount = 1 };
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello"),
            },
            new Box
            {
                new Text("world"),
            },
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
    public async Task TestChildrenUpdate()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello")
            },
            new Box
            {
                new Text("world")
            },
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
    public async Task TestChildAdd()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello")
            },
            new Box
            {
                new Text("hello"), 
                new Text("world")
            },
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
    public async Task TestChildRemove()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello"), 
                new Text("world")
            },
            new Box
            {
                new Text("hello")
            },
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
    public async Task TestChildAddUpdateReference()
    {
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var child = new TestControl { UpdateCount = 1 };
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello1")
            },
            new Box
            {
                new Text("hello2"), 
                new Text("world")
            },
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

// internal class TestControl
// {
//     public int UpdateCount { get; set; }
// }
//
// internal class TestPanel : TestControl
// {
//     public List<TestControl> Children { get; } = new();
// }
//
// internal class TestBuildOwner : IBuildOwner
// {
//     public List<Widget> RebuiltWidgets { get; } = new();
//
//     public bool NeedsBuild => RebuiltWidgets.Count > 0;
//
//
//     public void MarkNeedsBuild(Widget widget)
//     {
//         RebuiltWidgets.Add(widget);
//     }
//
//     public bool IsBuildScheduled(Widget widget)
//     {
//         return RebuiltWidgets.Contains(widget);
//     }
//
//     public void CleanUp()
//     {
//         RebuiltWidgets.Clear();
//     }
// }
//
// internal class TestWidgetBuilder : WidgetBuilder<TestControl>
// {
//     public TestWidgetBuilder(IBuildOwner owner) : base(owner)
//     {
//     }
//
//     protected override IRenderer<TestControl> GetRenderer(Type widgetType)
//     {
//         if (typeof(IPanelWidget).IsAssignableFrom(widgetType))
//         {
//             return new TestPanelRenderer();
//         }
//
//         return new TestRenderer();
//     }
// }
//
// internal class TestPanelRenderer : IPanelRenderer<TestControl>
// {
//     public TestControl Create()
//     {
//         return new TestPanel();
//     }
//
//     public void Update(TestControl control, MappingWidget widget)
//     {
//         control.UpdateCount++;
//     }
//
//     public void AddChild(TestControl control, TestControl childControl)
//     {
//         if (control is TestPanel panel)
//         {
//             panel.Children.Add(childControl);
//         }
//     }
//
//     public void RemoveChild(TestControl control, TestControl childControl)
//     {
//         if (control is TestPanel panel)
//         {
//             panel.Children.Remove(childControl);
//         }
//     }
//
//     public void ReplaceChild(TestControl control, int index, TestControl newChildControl)
//     {
//         if (control is TestPanel panel)
//         {
//             panel.Children[index] = newChildControl;
//         }
//     }
//
//     public TestControl? GetChildAt(TestControl control, int index)
//     {
//         return control is TestPanel panel ? panel.Children.ElementAtOrDefault(index) : null;
//     }
// }
//
// internal class TestRenderer : IRenderer<TestControl>
// {
//     public TestControl Create()
//     {
//         return new TestControl();
//     }
//
//     public void Update(TestControl control, MappingWidget widget)
//     {
//         control.UpdateCount++;
//     }
// }