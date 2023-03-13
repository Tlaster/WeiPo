using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class ColumnRenderer : RendererObject<Column, UIStackView>
{
    protected override UIStackView Create()
    {
        var outer = new UIStackView
        {
            Axis = UILayoutConstraintAxis.Horizontal,
            Alignment = UIStackViewAlignment.Top,
        };
        var inner = new UIStackView
        {
            Axis = UILayoutConstraintAxis.Vertical,
            Alignment = UIStackViewAlignment.Leading,
        };
        outer.AddArrangedSubview(inner);
        return outer;
    }

    protected override void Update(UIStackView control, Column widget)
    {
    }

    protected override void AddChild(UIStackView control, UIView childControl)
    {
        var realControl = control.ArrangedSubviews[0] as UIStackView;
        realControl?.AddArrangedSubview(childControl);
    }

    protected override void RemoveChild(UIStackView control, UIView childControl)
    {
        var realControl = control.ArrangedSubviews[0] as UIStackView;
        realControl?.RemoveArrangedSubview(childControl);
    }

    protected override void ReplaceChild(UIStackView control, int index, UIView newChildControl)
    {
        var realControl = control.ArrangedSubviews[0] as UIStackView;
        realControl?.ArrangedSubviews[index].RemoveFromSuperview();
        realControl?.InsertArrangedSubview(newChildControl, Convert.ToUInt32(index));
    }

    public override UIView? GetChildAt(UIView control, int index)
    {
        var realControl = control as UIStackView;
        var realInnerControl = realControl?.ArrangedSubviews[0] as UIStackView;
        return realInnerControl?.ArrangedSubviews[index];
    }
}