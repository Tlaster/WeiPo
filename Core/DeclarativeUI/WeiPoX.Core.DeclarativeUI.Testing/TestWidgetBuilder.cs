using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

internal class TestWidgetBuilder : WidgetBuilder<TestControl>
{
    private readonly IBuildOwner _owner;

    public TestWidgetBuilder(IBuildOwner owner)
    {
        _owner = owner;
    }
    
    public Task<TestControl> BuildIfNeededAsync(Widget? oldValue, Widget newValue, TestControl? control)
    {
        return Build(oldValue, newValue, control, _owner);
    }

    protected override IRenderer<TestControl> GetRenderer(Type widgetType)
    {
        if (typeof(IPanelWidget).IsAssignableFrom(widgetType))
        {
            return new TestPanelRenderer();
        }

        return new TestRenderer();
    }
}