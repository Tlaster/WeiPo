using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android;

public class DeclarativeView : FrameLayout
{
    private readonly DeclarativeCore<View> _core;

    public DeclarativeView(Context context, IBuildOwner? buildOwner = null) : base(context)
    {
        _core = new DeclarativeCore<View>(new WidgetBuilder(context), UpdateChild, buildOwner);
    }
    
    public Widget? Widget
    {
        get => _core.Widget;
        set => _core.Widget = value;
    }
    
    internal void UpdateChild(View view)
    {
        if (ChildCount == 0)
        {
            AddView(view);
        }
        else if (view != GetChildAt(0))
        {
            RemoveViewAt(0);
            AddView(view);
        }
    }
}