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
        tester.Render();
        tester.Render();
        var navHost = tester.GetWidget<AppWidget>().GetChild<ContextProvider>()?.FindChildAtIndex<TestNavHost>(0)?.GetChild<NavHost>();
        Assert.IsNotNull(navHost);
        var box = navHost.GetChild<Box>();
        Assert.IsNotNull(box);
        var text = box.FindChildAtIndex<ContextProvider>(0)?.FindChildAtIndex<Text>(0);
        Assert.IsNotNull(text);
        Assert.AreEqual("Home", text.Content);
        navigator.Push("detail");
        tester.Render();
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
            return new NavHost(
                Navigator,
                "home",
                ImmutableListOf(
                    new Route(
                        "home",
                        entry => Text("Home")
                    ),
                    new Route(
                        "detail",
                        entry => Text("Detail")
                    )
                )
            );
        }
    }
}

public record AppWidget(Widget Child) : StatefulWidget
{
    protected override Widget Build()
    {
        return ContextProvider(
            Providers(
                (typeof(StateHolder), new StateHolder()),
                (typeof(LifecycleHolder), new LifecycleHolder())
            ),
            Child
        );
    }
}