using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Internal;

internal interface IRenderer<T>
{
    T Create();
    void Update(T control, Widget widget);
    void AddChild(T control, T childControl);
    void RemoveChild(T control, T childControl);
    void ReplaceChild(T control, int index, T newChildControl);
}