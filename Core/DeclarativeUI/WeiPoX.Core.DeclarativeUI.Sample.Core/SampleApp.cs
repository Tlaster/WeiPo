using WeiPoX.Core.DeclarativeUI.Widgets;
using static WeiPoX.Core.DeclarativeUI.Widgets.FuncUi;

namespace WeiPoX.Core.DeclarativeUI.Sample.Core;

public record SampleApp : StatefulWidget
{
    protected override Widget Build()
    {
        var (value, setValue) = UseState(0);
        return Column(
            Button(
                text: "Click me!",
                onClick: () =>
                {
                    setValue(value + 1);
                }
            ),
            Text("Value: " + value)
        );
    }
}