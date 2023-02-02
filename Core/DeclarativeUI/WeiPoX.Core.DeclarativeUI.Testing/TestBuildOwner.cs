using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

internal class TestBuildOwner : IBuildOwner
{
    public List<Widget> RebuiltWidgets { get; } = new();

    public void MarkNeedsBuild(Widget widget)
    {
        RebuiltWidgets.Add(widget);
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return RebuiltWidgets.Contains(widget);
    }

    public void CleanUp()
    {
        RebuiltWidgets.Clear();
    }
}