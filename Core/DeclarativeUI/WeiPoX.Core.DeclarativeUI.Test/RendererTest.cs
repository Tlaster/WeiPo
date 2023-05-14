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
        var control = new TestPanel
        {
            UpdateCount = 0,
            Children =
            {
                new TestControl { UpdateCount = 1 }
            }
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
            new TestPanel
            {
                UpdateCount = 0,
                Children =
                {
                    new TestControl
                    {
                        UpdateCount = 1,
                    }
                }
            }
        );
        Assert.IsInstanceOfType<TestPanel>(result);
        var panel = (TestPanel)result;
        Assert.AreEqual(1, panel.UpdateCount);
        Assert.AreEqual(1, panel.Children.Count);
        Assert.AreEqual(2, panel.Children[0].UpdateCount);
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
            new TestPanel
            {
                UpdateCount = 1,
                Children =
                {
                    new TestControl { UpdateCount = 1 }
                }
            }
        );
        Assert.AreEqual(1, result.UpdateCount);
        Assert.AreEqual(1, ((TestPanel)result).Children.Count);
        Assert.AreEqual(1, ((TestPanel)result).Children[0].UpdateCount);
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

    [TestMethod]
    public async Task TestChildTreeChanged()
    {
        
        var owner = new TestBuildOwner();
        var renderer = new TestWidgetBuilder(owner);
        var result = await renderer.BuildIfNeededAsync(
            new Box
            {
                new Text("hello1")
            },
            new Box
            {
                new Box(),
                new Text("world")
            },
            new TestPanel
            {
                UpdateCount = 1,
                Children =
                {
                    new TestControl
                    {
                        UpdateCount = 1
                    }
                }
            }
        );
        Assert.IsInstanceOfType<TestPanel>(result);
        var panel = (TestPanel)result;
        Assert.AreEqual(2, panel.UpdateCount);
        Assert.AreEqual(2, panel.Children.Count);
        Assert.AreEqual(1, panel.Children[0].UpdateCount);
        Assert.AreEqual(1, panel.Children[1].UpdateCount);
        Assert.IsInstanceOfType<TestPanel>(panel.Children[0]);
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
    public void TestMultiPassTreeChanged()
    {
        var test = new TestMultiPassWidget().Test();
        var widget = test.GetWidget<TestMultiPassWidget>();
        var button = widget.GetChild<Box>()?.FindChildAtIndex<Button>(0);
        Assert.IsNotNull(button);
        var panel = test.GetControl<TestPanel>()?.Children[2];
        Assert.IsNotNull(panel);
        Assert.IsInstanceOfType<TestPanel>(panel);
        button.OnClick.Invoke();
        var control = test.GetControl<TestPanel>()?.Children[2];
        Assert.IsNotNull(control);
        Assert.IsInstanceOfType<TestControl>(control);
        button = widget.GetChild<Box>()?.FindChildAtIndex<Button>(0);
        Assert.IsNotNull(button);
        button.OnClick.Invoke();
        var control2 = test.GetControl<TestPanel>()?.Children[2];
        Assert.IsNotNull(control2);
        Assert.IsInstanceOfType<TestPanel>(control2);
    }
}
