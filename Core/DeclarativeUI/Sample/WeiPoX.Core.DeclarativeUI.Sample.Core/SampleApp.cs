using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Sample.Core;

public record SampleApp : StatefulWidget
{
    protected override Widget Build()
    {
        var (value, setValue) = UseState(0);
        var (text, setText) = UseState(new InputState("Hello World!"));
        return new LazyColumn
        {
            new Item
            {
                new Column
                {
                    Horizontal = Alignment.Horizontal.Center,
                    Vertical = Alignment.Vertical.Center,
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
            },
            new Items(100)
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

