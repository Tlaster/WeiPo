using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

internal class TestRenderer : IRenderer<TestControl>
{
    public TestControl Create(WidgetBuilder<TestControl> renderer)
    {
        return new TestControl();
    }

    public void Update(TestControl control, MappingWidget widget)
    {
        control.UpdateCount++;
    }

    public void AddChild(TestControl control, TestControl childControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children.Add(childControl);
        }
    }

    public void RemoveChild(TestControl control, TestControl childControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children.Remove(childControl);
        }
    }

    public void ReplaceChild(TestControl control, int index, TestControl newChildControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children[index] = newChildControl;
        }
    }
    
    public bool IsPanel(TestControl value)
    {
        return value is TestPanel;
    }

    public TestControl? GetChildAt(TestControl control, int index)
    {
        return control is TestPanel panel ? panel.Children.ElementAtOrDefault(index) : null;
    }
}