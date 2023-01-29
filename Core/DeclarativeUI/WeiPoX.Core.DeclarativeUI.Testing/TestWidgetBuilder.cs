using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

internal class TestWidgetBuilder : WidgetBuilder<TestControl>
{
    public TestWidgetBuilder(IBuildOwner owner) : base(owner)
    {
    }

    protected override IRenderer<TestControl> GetRenderer(Type widgetType)
    {
        if (typeof(IPanelWidget).IsAssignableFrom(widgetType))
        {
            return new TestPanelRenderer();
        }

        return new TestRenderer();
    }

    protected override bool IsPanel(TestControl value)
    {
        return value is TestPanel;
    }

    protected override TestControl? GetChildAt(TestControl control, int index)
    {
        return control is TestPanel panel ? panel.Children.ElementAtOrDefault(index) : null;
    }
}