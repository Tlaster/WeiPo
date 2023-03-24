using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

internal interface IRenderer<T>
{
    T Create(WidgetBuilder<T> renderer);
    void Update(T control, MappingWidget widget);
    void AddChild(T control, T childControl);
    void RemoveChild(T control, T childControl);
    void ReplaceChild(T control, int index, T newChildControl);
    T? GetChildAt(T control, int index);
    bool IsPanel(T value);
}

internal interface ILazyRenderer<T>
{
    bool IsVisible(T control, int index);
    T? GetVisibleChild(T control, int index);
    void UpdateChild(T control, int index, T childControl);
}

internal interface IBuildOwner
{
    void MarkNeedsBuild(Widget widget);

    bool IsBuildScheduled(Widget widget);

    void CleanUp();
}