using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

record HookUseStateWidget: StatefulWidget
{
    protected override Widget Build()
    {
        var (value, setValue) = UseState(0);
        return Row(
            Button(
                () => { setValue.Invoke(value + 1); },
                Text("Click")
            ),
            Text(value.ToString())
        );
    }
}

record HookUseEffectWidget(Action Action): StatefulWidget
{
    protected override Widget Build()
    {
        var (value, setValue) = UseState(0);
        UseEffect(Action.Invoke, value);
        return Row(
            Button(
                () => { setValue.Invoke(value + 1); },
                Text("Click")
            ),
            Text(value.ToString())
        );
    }
}

[TestClass]
public class HooksTest
{
    [TestMethod]
    public void TestHookUseStateWidget()
    {
        var widget = new HookUseStateWidget();
        var build = widget.BuildInternal();
        var row = (Row)build;
        var button = (Button)row.Children[0];
        var text = (Text)row.Children[1];
        Assert.IsTrue("0" == text.Content);
        button.OnClick();
        var newBuild = widget.BuildInternal();
        var newRow = (Row)newBuild;
        var newText = (Text)newRow.Children[1];
        Assert.IsTrue("1" == newText.Content);
    }

    [TestMethod]
    public void TestHooksUseEffectWidget()
    {
        var count = 0;
        var widget = new HookUseEffectWidget(() => { count++; });
        var build = widget.BuildInternal();
        var row = (Row)build;
        var button = (Button)row.Children[0];
        Assert.IsTrue(count == 1);
        button.OnClick();
        widget.BuildInternal();
        Assert.IsTrue(count == 2);
        widget.BuildInternal();
        Assert.IsTrue(count == 2);
    }
}