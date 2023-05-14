using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Animation;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using WeiPoX.Core.Lifecycle;
using WeiPoX.Core.Routing;

namespace WeiPoX.Core.DeclarativeUI.Sample.Core;

public record SampleApp : StatefulWidget
{
    protected override Widget Build()
    {
        var navigator = this.UseNavigator();
        return new Column
        {
            new Button
            {
                Text = "Back",
                OnClick = () => navigator.Pop()
            },
            new NavHost
            {
                Navigator = navigator,
                InitialRoute = "home",
                Routes = new[]
                {
                    new Route
                    {
                        Path = "home",
                        Content = entry => new HomeScene(new HomeNavigation(
                            OnCounter: () => navigator.Push("counter"),
                            OnLazyColumn: () => navigator.Push("lazyColumn"),
                            OnAnimatedVisibility: () => navigator.Push("animatedVisibility"))
                        )
                    },
                    new Route
                    {
                        Path = "counter",
                        Content = entry => new CounterScene(() => navigator.Push("detail"))
                    },
                    new Route
                    {
                        Path = "lazyColumn",
                        Content = entry => new LazyColumnScene()
                    },
                    new Route
                    {
                        Path = "animatedVisibility",
                        Content = entry => new AnimatedVisibilityScene()
                    },
                    new Route
                    {
                        Path = "detail",
                        Content = entry => new DetailScene(() => navigator.Pop())
                    }
                }.ToImmutableList()
            }
        };
    }
}

public record HomeNavigation(Action OnCounter, Action OnLazyColumn, Action OnAnimatedVisibility);

public record HomeScene(HomeNavigation HomeNavigation) : StatefulWidget
{
    protected override Widget Build()
    {
        return new Column
        {
            new GestureDetector
            {
                Children =
                {
                    new Text("CounterScene"),
                },
                OnTap = HomeNavigation.OnCounter,
            },
            new GestureDetector
            {
                Children =
                {
                    new Text("LazyColumnScene"),
                },
                OnTap = HomeNavigation.OnLazyColumn,
            },
            new GestureDetector
            {
                Children =
                {
                    new Text("AnimatedVisibilityScene"),
                },
                OnTap = HomeNavigation.OnAnimatedVisibility,
            },
        };
    }
}

public record AnimatedVisibilityScene : StatefulWidget
{
    protected override Widget Build()
    {
        var (show, setShow) = UseState(false);
        return new Column
        {
            new Text("Hello World!"),
            new Button
            {
                Text = "Click me!",
                OnClick = () => setShow(!show)
            },
            new AnimatedVisibility
            {
                Visible = show,
                Child = new Text("Hello World!")
            },
            new Text("haha!"),
        };
    }
}

public record LazyColumnScene : StatefulWidget
{
    protected override Widget Build()
    {
        var (value, setValue) = UseState(0);
        var (text, setText) = UseState(new InputState("Hello World!"));
        return new LazyColumn
        {
            new Item
            {
                new Box
                {
                    BackgroundColor = new Color("#FF0000"),
                    Height = 200,
                    Horizontal = Alignment.Horizontal.Center,
                    Vertical = Alignment.Vertical.Center,
                    Children =
                    {
                        new Column
                        {
                            Alignment = Alignment.Horizontal.Center,
                            Children =
                            {
                                new Button
                                {
                                    Text = "Click me!",
                                    OnClick = () => setValue(value + 1)
                                },
                                new Text("Value: " + value),
                                new Input
                                {
                                    State = text,
                                    OnStateChanged = setText
                                }
                            }
                        }
                    }
                }
            },
            new Items(10000)
            {
                Builder = index => new Column
                {
                    new Text($"item {index}, value: {value}"),
                    new Text($"text: {text.Text}")
                }
            }
        };
    }
}

public record CounterScene(Action OnNavigate) : StatefulWidget
{
    protected override Widget Build()
    {
        var (value, setValue) = UseSaveableState("key", 0);
        return new Column
        {
            new Text("Home"),
            new Text("Value: " + value),
            new GestureDetector
            {
                OnTap = () => setValue(value + 1),
                Children =
                {
                    new Box
                    {
                        Padding = new Thickness(16),
                        BackgroundColor = new Color("#FF0000"),
                        Children =
                        {
                            new Text("Click me!")
                        }
                    }
                }
            },
            new Button
            {
                Text = "Click me!",
                OnClick = () => setValue(value + 1)
            },
            new Button
            {
                Text = "Go Detail",
                OnClick = OnNavigate
            }
        };
    }
}

public record DetailScene(Action OnNavigate) : StatefulWidget
{
    protected override Widget Build()
    {
        return new Column
        {
            new Text("Detail"),
            new Button
            {
                Text = "Go Back",
                OnClick = OnNavigate
            }
        };
    }
}