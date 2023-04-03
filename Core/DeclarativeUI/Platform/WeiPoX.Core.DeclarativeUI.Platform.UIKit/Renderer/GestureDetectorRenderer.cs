using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class GestureDetectorRenderer : RendererObject<GestureDetector, UIView>
{
    protected override void Update(UIView control, GestureDetector widget)
    {
        control.UserInteractionEnabled = true;
        //clear all gesture recognizers
        if (control.GestureRecognizers != null)
        {
            foreach (var gestureRecognizer in control.GestureRecognizers)
            {
                control.RemoveGestureRecognizer(gestureRecognizer);
            }
        }

        if (widget.OnTap != null)
        {
            control.AddGestureRecognizer(new UITapGestureRecognizer(() => widget.OnTap.Invoke()));
        }
        if (widget.OnLongPress != null)
        {
            control.AddGestureRecognizer(new UILongPressGestureRecognizer(() => widget.OnLongPress.Invoke()));
        }
        if (widget.OnDoubleTap != null)
        {
            control.AddGestureRecognizer(new UITapGestureRecognizer(() => widget.OnDoubleTap.Invoke())
            {
                NumberOfTapsRequired = 2
            });
        }
    }
}