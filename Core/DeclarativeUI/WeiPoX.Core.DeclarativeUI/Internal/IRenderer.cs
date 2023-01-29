using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

internal interface IRenderer<T>
{
    T Create();
    void Update(T control, MappingWidget widget);
    void AddChild(T control, T childControl);
    void RemoveChild(T control, T childControl);
    void ReplaceChild(T control, int index, T newChildControl);
}

internal interface IBuildOwner
{
    void MarkNeedsBuild(Widget widget);

    bool IsBuildScheduled(Widget widget);

    void CleanUp();
}