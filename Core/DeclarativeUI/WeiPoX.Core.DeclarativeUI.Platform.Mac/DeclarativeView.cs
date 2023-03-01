using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Mac.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Mac;

public abstract class DeclarativeView : UIViewController, IBuildOwner
{
    public List<Widget> RebuiltWidgets { get; } = new();
    private readonly WidgetBuilder _renderer;
    private UIView? _content;
    private bool _rendering;
    private bool _requireReRender;

    protected DeclarativeView()
    {
        _renderer = new WidgetBuilder(this);
    }

    public void MarkNeedsBuild(Widget widget)
    {
        RebuiltWidgets.Add(widget);
        if (_rendering)
        {
            _requireReRender = true;
        }
        else
        {
            Render();
        }
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return RebuiltWidgets.Contains(widget);
    }

    public void CleanUp()
    {
        RebuiltWidgets.Clear();
    }

    protected void Render()
    {
        try
        {
            _rendering = true;
            _content = _renderer.BuildIfNeeded(Content, Content, _content);
            _rendering = false;
            if (!_requireReRender)
            {
                if (View == null)
                {
                    return;
                }

                if (View.Subviews.Length == 0)
                {
                    View.AddSubview(_content);
                    ApplySafeArea();
                }
                else if (!View.Subviews[0].Equals(_content))
                {
                    View.Subviews[0].RemoveFromSuperview();
                    View.AddSubview(_content);
                    ApplySafeArea();
                }
                return;
            }

            _requireReRender = false;
            Render();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void ApplySafeArea()
    {
        if (_content == null)
        {
            return;
        }
        _content.TranslatesAutoresizingMaskIntoConstraints = false;
        var guide = View?.LayoutMarginsGuide;
        if (guide != null)
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _content.LeadingAnchor.ConstraintEqualTo(guide.LeadingAnchor),
                _content.TrailingAnchor.ConstraintEqualTo(guide.TrailingAnchor),
            });
        }
        var safeGuide = View?.SafeAreaLayoutGuide;
        if (safeGuide != null)
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _content.TopAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.TopAnchor, 1),
                _content.BottomAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.BottomAnchor, 1),
            });
        }
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        if (View != null)
        {
            View.BackgroundColor = UIColor.SystemBackground;
        }
    }

    protected abstract Widget Content { get; }
}

public class Declarative : DeclarativeView
{
    public Declarative(Widget content)
    {
        Content = content;
        Render();
    }

    protected override Widget Content { get; }
}