using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Testing;

internal class TestBuildOwner : IBuildOwner
{
    public event Action? OnRequestBuild;
    public List<Widget> RebuiltWidgets { get; } = new();
    public bool NeedsBuild => RebuiltWidgets.Count > 0;

    public void MarkNeedsBuild(Widget widget)
    {
        RebuiltWidgets.Add(widget);
        OnRequestBuild?.Invoke();
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