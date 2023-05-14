using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

internal class TestPanelRenderer : IRenderer<TestControl>
{
    public TestControl Create(RendererContext<TestControl> context)
    {
        return new TestPanel();
    }

    public void Update(TestControl control, MappingWidget widget)
    {
        if (control is TestPanel panel)
        {
            control.UpdateCount++;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public void AddChild(TestControl control, TestControl childControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children.Add(childControl);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public void RemoveChild(TestControl control, TestControl childControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children.Remove(childControl);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public void ReplaceChild(TestControl control, int index, TestControl newChildControl)
    {
        if (control is TestPanel panel)
        {
            panel.Children[index] = newChildControl;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
    
    public bool IsPanel(TestControl value)
    {
        return value is TestPanel;
    }

    public TestControl? GetChildAt(TestControl control, int index)
    {
        return control is TestPanel panel ? panel.Children.ElementAtOrDefault(index) : throw new InvalidOperationException();
    }
}