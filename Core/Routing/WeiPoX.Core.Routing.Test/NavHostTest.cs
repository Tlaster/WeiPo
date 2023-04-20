using System.Collections.Immutable;
using Splat;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Testing;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.Routing.Test;

[TestClass]
public class NavHostTest
{
    [TestMethod]
    public void TestNavigation()
    {
        var navigator = new Navigator();
        var tester = new AppWidget(new TestNavHost(navigator)).Test();
        var navHost = tester.GetWidget<AppWidget>().GetChild<ContextProvider>()?.FindChildAtIndex<TestNavHost>(0)?.GetChild<NavHost>();
        Assert.IsNotNull(navHost);
        var box = navHost.GetChild<Box>();
        Assert.IsNotNull(box);
        var text = box.FindChildAtIndex<ContextProvider>(0)?.FindChildAtIndex<Text>(0);
        Assert.IsNotNull(text);
        Assert.AreEqual("Home", text.Content);
        navigator.Push("detail");
        navHost = tester.GetWidget<AppWidget>().GetChild<ContextProvider>()?.FindChildAtIndex<TestNavHost>(0)?.GetChild<NavHost>();
        Assert.IsNotNull(navHost);
        box = navHost.GetChild<Box>();
        Assert.IsNotNull(box);
        text = box.FindChildAtIndex<ContextProvider>(0)?.FindChildAtIndex<Text>(0);
        Assert.IsNotNull(text);
        Assert.AreEqual("Detail", text.Content);
    }

    record TestNavHost(Navigator Navigator) : StatefulWidget
    {
        protected override Widget Build()
        {
            return new NavHost
            {
                Navigator = Navigator,
                InitialRoute = "home",
                Routes = new []
                {
                    new Route
                    {
                      Path  = "home",
                      Content = entry => new Text("Home")
                    },
                    new Route
                    {
                      Path  = "detail",
                      Content = entry => new Text("Detail")
                    }
                }.ToImmutableList()
            };
        }
    }

    record NestedStateWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return new Column
            {
                new Text(value.ToString()),
                new Button
                {
                    Text = "Increment",
                    OnClick = () => setValue(value + 1)
                }
            };
        }
    }
    
    record NestedHostWidget(Navigator Navigator) : StatefulWidget
    {
        protected override Widget Build()
        {
            return new NavHost
            {
                Navigator = Navigator,
                InitialRoute = "home",
                Routes = new[]
                {
                    new Route
                    {
                        Path = "home",
                        Content = entry => new NestedStateWidget()
                    },
                }.ToImmutableList()
            };
        }
    }
    
    [TestMethod]
    public void TestNestedNavigation()
    {
        var navigator = new Navigator();
        var tester = new AppWidget(new NestedHostWidget(navigator)).Test();
        var navHost = tester.GetWidget<AppWidget>().GetChild<ContextProvider>()?.FindChildAtIndex<NestedHostWidget>(0)?.GetChild<NavHost>();
        Assert.IsNotNull(navHost);
        var box = navHost.GetChild<Box>();
        Assert.IsNotNull(box);
        var nestedStateWidget = box.FindChildAtIndex<ContextProvider>(0)?.FindChildAtIndex<NestedStateWidget>(0);
        Assert.IsNotNull(nestedStateWidget);
        var text = nestedStateWidget.GetChild<Column>()?.FindChildAtIndex<Text>(0);
        Assert.IsNotNull(text);
        Assert.AreEqual("0", text.Content);
        var button = nestedStateWidget.GetChild<Column>()?.FindChildAtIndex<Button>(1);
        Assert.IsNotNull(button);
        button.OnClick();
        text = nestedStateWidget.GetChild<Column>()?.FindChildAtIndex<Text>(0);
        Assert.IsNotNull(text);
        Assert.AreEqual("1", text.Content);
    }
}

public record AppWidget(Widget Child) : StatefulWidget
{
    protected override Widget Build()
    {
        return new ContextProvider
        {
            Providers =
            {
                {typeof(StateHolder), new StateHolder()},
                {typeof(LifecycleHolder), new LifecycleHolder()},
                {typeof(BackDispatcher), new BackDispatcher()}
            },
            Child = Child,
        };
    }
}