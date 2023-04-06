using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

public interface IRenderer<T>
{
    T Create(RendererContext<T> context);
    void Update(T control, MappingWidget widget);
    void AddChild(T control, T childControl);
    void RemoveChild(T control, T childControl);
    void ReplaceChild(T control, int index, T newChildControl);
    T? GetChildAt(T control, int index);
    bool IsPanel(T value);
}

public record RendererContext<T>(WidgetBuilder<T> Renderer, IBuildOwner BuildOwner);

public interface ILazyRenderer<T>
{
    Range GetVisibleRange(T control);
    bool IsVisible(T control, int index);
    T? GetVisibleChild(T control, int index);
    void UpdateChild(T control, int index, T childControl);
}

public interface IBuildOwner
{
    void MarkNeedsBuild(Widget widget);

    bool IsBuildScheduled(Widget widget);

    void CleanUp();
}