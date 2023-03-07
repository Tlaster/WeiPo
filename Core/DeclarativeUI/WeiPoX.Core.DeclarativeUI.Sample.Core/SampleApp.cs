using WeiPoX.Core.DeclarativeUI.Widgets;
using static WeiPoX.Core.DeclarativeUI.Widgets.FuncUi;

namespace WeiPoX.Core.DeclarativeUI.Sample.Core;

public record SampleApp : StatefulWidget
{
    protected override Widget Build()
    {
        var (value, setValue) = UseState(0);
        var (text, setText) = UseState(new InputState("Hello World!"));
        return Column(
            Button(
                text: "Click me!",
                onClick: () =>
                {
                    setValue(value + 1);
                }
            ),
            Text("Value: " + value),
            Input(
                text: text,
                onTextChanged: newValue =>
                {
                    if (!newValue.Text.EndsWith('0'))
                    {
                        setText(newValue);
                    }
                    else
                    {
                        setText(newValue with { Text = text.Text });
                    }
                }
            ),
            Text("Text: " + text.Text)
        );
    }
}