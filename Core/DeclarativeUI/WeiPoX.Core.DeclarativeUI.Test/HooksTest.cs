using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class HooksTest
{
    [TestMethod]
    public void TestHookUseStateWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new HookUseStateWidget();
        widget.State.BuildOwner = owner;
        var build = widget.BuildInternal();
        owner.CleanUp();
        var row = (Row)build;
        var button = (Button)row.Children[0];
        var text = (Text)row.Children[1];
        Assert.IsTrue("0" == text.Content);
        button.OnClick();
        var newBuild = widget.BuildInternal();
        owner.CleanUp();
        var newRow = (Row)newBuild;
        var newText = (Text)newRow.Children[1];
        Assert.IsTrue("1" == newText.Content);
    }

    [TestMethod]
    public void TestHooksUseEffectWidget()
    {
        var owner = new TestBuildOwner();
        var count = 0;
        var widget = new HookUseEffectWidget(() => { count++; });
        widget.State.BuildOwner = owner;
        var build = widget.BuildInternal();
        owner.CleanUp();
        var row = (Row)build;
        var button = (Button)row.Children[0];
        Assert.IsTrue(count == 1);
        button.OnClick();
        widget.BuildInternal();
        owner.CleanUp();
        Assert.IsTrue(count == 2);
        widget.BuildInternal();
        owner.CleanUp();
        Assert.IsTrue(count == 2);
    }

    [TestMethod]
    public void TestHooksUseEffectDisposableWidget()
    {
        var owner = new TestBuildOwner();
        var count = 0;
        var disposableCount = 0;
        var widget = new HookUseEffectDisposableWidget(() =>
        {
            count++;
            return () => { disposableCount++; };
        });
        widget.State.BuildOwner = owner;
        var build = widget.BuildInternal();
        owner.CleanUp();
        var row = (Row)build;
        var button = (Button)row.Children[0];
        Assert.AreEqual(1, count);
        button.OnClick();
        widget.BuildInternal();
        owner.CleanUp();
        Assert.AreEqual(2, count);
        Assert.AreEqual(1, disposableCount);
    }

    [TestMethod]
    public void TestHooksUseMemoWidget()
    {
        var owner = new TestBuildOwner();
        var count = 0;
        var widget = new HookUseMemoWidget(i =>
        {
            count++;
            return i * 2;
        });
        widget.State.BuildOwner = owner;
        var build = widget.BuildInternal();
        owner.CleanUp();
        var row = (Row)build;
        var button = (Button)row.Children[0];
        var text = (Text)row.Children[1];
        Assert.AreEqual(1, count);
        Assert.AreEqual("0", text.Content);
        button.OnClick();
        build = widget.BuildInternal();
        owner.CleanUp();
        Assert.AreEqual(2, count);
        row = (Row)build;
        text = (Text)row.Children[1];
        Assert.AreEqual("2", text.Content);
        build = widget.BuildInternal();
        owner.CleanUp();
        Assert.AreEqual(2, count);
        row = (Row)build;
        text = (Text)row.Children[1];
        Assert.AreEqual("2", text.Content);
    }

    private record HookUseStateWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return new Row
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => { setValue.Invoke(value + 1); }
                },
                new Text(value.ToString())
            };
        }
    }

    private record HookUseEffectWidget(Action Action) : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            UseEffect(Action.Invoke, value);
            return new Row
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => { setValue.Invoke(value + 1); }
                },
                new Text(value.ToString())
            };
        }
    }

    private record HookUseEffectDisposableWidget(Func<Action> Action) : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            UseEffect(() =>
            {
                var result = Action.Invoke();
                return result;
            }, value);
            return new Row
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => { setValue.Invoke(value + 1); }
                },
                new Text(value.ToString())
            };
        }
    }

    private record HookUseMemoWidget(Func<int, int> Memo) : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            var memo = UseMemo(() => Memo.Invoke(value), value);
            return new Row
            {
                new Button
                {
                    Text = "Click",
                    OnClick = () => { setValue.Invoke(value + 1); }
                },
                new Text(memo.ToString())
            };
        }
    }
}