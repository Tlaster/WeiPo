using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Test;

[TestClass]
public class StateTest
{
    [TestMethod]
    public void TestNestedWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new TestStatelessWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        Assert.IsInstanceOfType(result, typeof(TestPanel));
        var panel = (TestPanel)result;
        Assert.AreEqual(2, panel.Children.Count);
        Assert.IsInstanceOfType(panel.Children[0], typeof(TestControl));
        Assert.IsInstanceOfType(panel.Children[1], typeof(TestPanel));
        var panel2 = (TestPanel)panel.Children[1];
        Assert.AreEqual(1, panel2.Children.Count);
        Assert.IsInstanceOfType(panel2.Children[0], typeof(TestControl));
    }

    [TestMethod]
    public void TestStatefulState()
    {
        var owner = new TestBuildOwner();
        var widget = new TestUseStateWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        Assert.IsInstanceOfType(result, typeof(TestPanel));
        var panel = (TestPanel)result;
        Assert.AreEqual(2, panel.Children.Count);
        Assert.IsInstanceOfType(panel.Children[0], typeof(TestPanel));
        Assert.IsInstanceOfType(panel.Children[1], typeof(TestControl));
        var panel2 = (TestPanel)panel.Children[0];
        Assert.AreEqual(1, panel2.Children.Count);
        Assert.IsInstanceOfType(panel2.Children[0], typeof(TestControl));

        var cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        var row = (Row)cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(Text));
        var button = (Button)row.Children[0];
        var text = (Text)row.Children[1];
        Assert.AreEqual(1, button.Children.Count);
        Assert.AreEqual("0", text.Content);
        Assert.IsInstanceOfType(button.Children[0], typeof(Text));
        var text2 = (Text)button.Children[0];
        Assert.AreEqual("Click", text2.Content);

        Assert.IsFalse(owner.NeedsBuild);
        button.OnClick.Invoke();
        Assert.IsTrue(owner.NeedsBuild);
        Assert.AreEqual(1, owner.RebuiltWidgets.Count);
        var newResult = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        Assert.IsFalse(owner.NeedsBuild);
        Assert.AreSame(result, newResult);

        var newCache = widget.State.CachedBuild;
        Assert.IsNotNull(newCache);
        Assert.AreNotSame(cache, newCache);
        Assert.IsInstanceOfType(newCache, typeof(Row));
        var newRow = (Row)newCache;
        Assert.AreEqual(2, newRow.Children.Count);
        Assert.IsInstanceOfType(newRow.Children[0], typeof(Button));
        Assert.IsInstanceOfType(newRow.Children[1], typeof(Text));
        var newButton = (Button)newRow.Children[0];
        var newText = (Text)newRow.Children[1];
        Assert.AreEqual(1, newButton.Children.Count);
        Assert.AreEqual("1", newText.Content);
        Assert.IsInstanceOfType(newButton.Children[0], typeof(Text));
        var newText2 = (Text)newButton.Children[0];
        Assert.AreEqual("Click", newText2.Content);
    }

    [TestMethod]
    public void TestStatefulWidgetDispose()
    {
        var owner = new TestBuildOwner();
        var widget = new TestDisposeWidget();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        var cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        var row = (Row)cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(Text));
        var button = (Button)row.Children[0];
        button.OnClick.Invoke();
        result = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        row = (Row)cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(TestUseStateWidget));
        var stateWidget = (TestUseStateWidget)row.Children[1];
        Assert.IsFalse(stateWidget.Disposed);
        button = (Button)row.Children[0];
        button.OnClick.Invoke();
        result = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        row = (Row)cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(Text));
        Assert.IsTrue(stateWidget.Disposed);
    }

    [TestMethod]
    public void TestWithNestedState()
    {
        var owner = new TestBuildOwner();
        var widget = new TestStateHolder();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        var cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        var row = (Row)cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(TestStateReceiver));
        var button = (Button)row.Children[0];
        var receiver = (TestStateReceiver)row.Children[1];
        Assert.AreEqual(0, receiver.Value);
        var receiverCachedBuild = receiver.State.CachedBuild;
        Assert.IsNotNull(receiverCachedBuild);
        Assert.IsInstanceOfType(receiverCachedBuild, typeof(Text));
        var text = (Text)receiverCachedBuild;
        Assert.AreEqual("0", text.Content);

        button.OnClick.Invoke();
        result = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType(cache, typeof(Row));
        row = (Row)cache;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType(row.Children[0], typeof(Button));
        Assert.IsInstanceOfType(row.Children[1], typeof(TestStateReceiver));
        button = (Button)row.Children[0];
        var receiver2 = (TestStateReceiver)row.Children[1];
        Assert.AreNotSame(receiver, receiver2);
        Assert.AreEqual(1, receiver2.Value);
        receiverCachedBuild = receiver2.State.CachedBuild;
        Assert.IsNotNull(receiverCachedBuild);
        Assert.IsInstanceOfType(receiverCachedBuild, typeof(Text));
        text = (Text)receiverCachedBuild;
        Assert.AreEqual("1", text.Content);
    }

    [TestMethod]
    public void TestHooksUseContextWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new TestHookContextHolder();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        var cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType<ContextProvider>(cache);
        var contextProvider = (ContextProvider)cache;
        Assert.AreEqual(1, contextProvider.Providers.Count);
        Assert.AreEqual(typeof(TestHookContext), contextProvider.Providers.First().Key);
        Assert.AreEqual(0, ((TestHookContext)contextProvider.Providers.First().Value).Value);
        Assert.IsInstanceOfType<Row>(contextProvider.Child);
        var row = (Row)contextProvider.Child;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType<Button>(row.Children[0]);
        Assert.IsInstanceOfType<TestHookContextConsumer>(row.Children[1]);
        var button = (Button)row.Children[0];
        var consumer = (TestHookContextConsumer)row.Children[1];
        var consumerCachedBuild = consumer.State.CachedBuild;
        Assert.IsNotNull(consumerCachedBuild);
        Assert.IsInstanceOfType<Text>(consumerCachedBuild);
        var text = (Text)consumerCachedBuild;
        Assert.AreEqual("0", text.Content);

        button.OnClick.Invoke();
        result = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        Assert.IsInstanceOfType<ContextProvider>(cache);
        contextProvider = (ContextProvider)cache;
        Assert.AreEqual(1, contextProvider.Providers.Count);
        Assert.AreEqual(typeof(TestHookContext), contextProvider.Providers.First().Key);
        Assert.AreEqual(1, ((TestHookContext)contextProvider.Providers.First().Value).Value);
        Assert.IsInstanceOfType<Row>(contextProvider.Child);
        row = (Row)contextProvider.Child;
        Assert.AreEqual(2, row.Children.Count);
        Assert.IsInstanceOfType<Button>(row.Children[0]);
        Assert.IsInstanceOfType<TestHookContextConsumer>(row.Children[1]);
        button = (Button)row.Children[0];
        var consumer2 = (TestHookContextConsumer)row.Children[1];
        Assert.AreNotSame(consumer, consumer2);
        consumerCachedBuild = consumer2.State.CachedBuild;
        Assert.IsNotNull(consumerCachedBuild);
        Assert.IsInstanceOfType<Text>(consumerCachedBuild);
        text = (Text)consumerCachedBuild;
        Assert.AreEqual("1", text.Content);
    }

    [TestMethod]
    public void TestHooksUseContextWidgetWithNonContextWidget()
    {
        var owner = new TestBuildOwner();
        var widget = new TestHookContextHolderWithNonContext();
        var builder = new TestWidgetBuilder(owner);
        var result = builder.BuildIfNeeded(null, widget, null);
        var cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        var contextProvider = (ContextProvider)cache;
        var row = (Row)contextProvider.Child;
        var nonContextWidget = (TestHooksNonContextWidget)row.Children[2];
        var nonContextWidgetCachedBuild = nonContextWidget.State.CachedBuild;
        Assert.IsNotNull(nonContextWidgetCachedBuild);
        Assert.IsInstanceOfType<Text>(nonContextWidgetCachedBuild);
        var text = (Text)nonContextWidgetCachedBuild;
        Assert.AreEqual("hello", text.Content);

        var button = (Button)row.Children[0];
        button.OnClick.Invoke();
        result = builder.BuildIfNeeded(widget, widget, result);
        owner.CleanUp();
        cache = widget.State.CachedBuild;
        Assert.IsNotNull(cache);
        contextProvider = (ContextProvider)cache;
        row = (Row)contextProvider.Child;
        var nonContextWidget2 = (TestHooksNonContextWidget)row.Children[2];
        Assert.AreNotSame(nonContextWidget, nonContextWidget2);
        Assert.AreEqual(nonContextWidget, nonContextWidget2);
    }

    private record TestUseStateWidget : StatefulWidget
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

    private record TestStatelessWidget : StatelessWidget
    {
        protected internal override Widget Content { get; } = Column(
            Text("Hello"),
            new NestedStatelessWidget()
        );
    }

    private record NestedStatelessWidget : StatelessWidget
    {
        protected internal override Widget Content { get; } = Column(Text("World"));
    }

    private record TestDisposeWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return Row(
                Button(
                    () => { setValue.Invoke(value + 1); },
                    Text("Click")
                ),
                value == 1 ? new TestUseStateWidget() : Text(value.ToString())
            );
        }
    }

    private record TestStateHolder : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return Row(
                Button(
                    () => { setValue.Invoke(value + 1); },
                    Text("Click")
                ),
                new TestStateReceiver(value)
            );
        }
    }

    private record TestStateReceiver(int Value) : StatefulWidget
    {
        protected override Widget Build()
        {
            return Text(Value.ToString());
        }
    }


    private record TestHookContext(int Value);

    private record TestHookContextHolder : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return ContextProvider(new[]
                {
                    (typeof(TestHookContext), new TestHookContext(value) as object)
                },
                Row(
                    Button(
                        () => { setValue.Invoke(value + 1); },
                        Text("Click")
                    ),
                    new TestHookContextConsumer()
                ));
        }
    }

    private record TestHookContextConsumer : StatefulWidget
    {
        protected override Widget Build()
        {
            var context = UseContext<TestHookContext>();
            return Text(context.Value.ToString());
        }
    }

    private record TestHookContextHolderWithNonContext : StatefulWidget
    {
        protected override Widget Build()
        {
            var (value, setValue) = UseState(0);
            return ContextProvider(new[]
            {
                (typeof(TestHookContext), new TestHookContext(value) as object)
            }, Row(
                Button(
                    () => { setValue.Invoke(value + 1); },
                    Text("Click")
                ),
                new TestHookContextConsumer(),
                new TestHooksNonContextWidget()
            ));
        }
    }

    private record TestHooksNonContextWidget : StatefulWidget
    {
        protected override Widget Build()
        {
            return Text("hello");
        }
    }
}