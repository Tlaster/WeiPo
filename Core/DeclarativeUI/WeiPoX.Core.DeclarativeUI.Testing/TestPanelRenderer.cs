using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

internal class TestPanelRenderer : IRenderer<TestControl>
{
    public TestControl Create()
    {
        return new TestPanel();
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
}