using WeiPoX.Core.DeclarativeUI.Widget;

namespace WeiPoX.Core.DeclarativeUI.Internal;

internal interface IRenderer<T>
{
    T Create();
    void Update(T control, WidgetObject widget);
    void AddChild(T control, T childControl);
    void RemoveChild(T control, T childControl);
    void ReplaceChild(T control, int index, T newChildControl);
}