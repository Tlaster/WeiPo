using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class HooksTest
{
    private Widget HookUseStateWidget()
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

    private Widget HookUseEffectWidget(Action action) 
    {
        var (value, setValue) = UseState(0);
        UseEffect(action.Invoke, value);
        return Row(
            Button(
                () => { setValue.Invoke(value + 1); },
                Text("Click")
            ),
            Text(value.ToString())
        );
    }

    [TestMethod]
    public void TestHookUseStateWidget()
    {
        var widget = RenderWithHooks(HookUseStateWidget);
        var row = (Row)widget;
        var button = (Button)row.Children[0];
        var text = (Text)row.Children[1];
        Assert.IsTrue("0" == text.Content);
        button.OnClick();
        var newWidget = RenderWithHooks(HookUseStateWidget);
        var newRow = (Row)newWidget;
        var newText = (Text)newRow.Children[1];
        Assert.IsTrue("1" == newText.Content);
    }

    [TestMethod]
    public void TestHooksUseEffectWidget()
    {
        var count = 0;
        var widget = RenderWithHooks(() => HookUseEffectWidget(() => { count++; }));
        var row = (Row)widget;
        var button = (Button)row.Children[0];
        Assert.IsTrue(count == 1);
        button.OnClick();
        RenderWithHooks(() => HookUseEffectWidget(() => { count++; }));
        Assert.IsTrue(count == 2);
        RenderWithHooks(() => HookUseEffectWidget(() => { count++; }));
        Assert.IsTrue(count == 2);
    }
}