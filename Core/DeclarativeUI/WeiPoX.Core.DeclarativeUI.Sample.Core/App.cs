using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using static WeiPoX.Core.DeclarativeUI.Widgets.FuncUi;

namespace WeiPoX.Core.DeclarativeUI.Sample.Core;

public record App : StatefulWidget
{
    protected override Widget Build()
    {
        var (value, setValue) = UseState(0);
        return Column(
            Button(
                onClick: () => { },
                Text("Click me")
            ),
            Text("Value: " + value)
        );
    }
}