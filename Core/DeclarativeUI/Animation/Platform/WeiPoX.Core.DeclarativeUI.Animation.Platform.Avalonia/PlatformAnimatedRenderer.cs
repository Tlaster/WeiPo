using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Animators;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Avalonia.Styling;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Animation.Platform.Avalonia;

public class PlatformAnimatedRenderer : BoxRenderer
{
    protected override WeiPoXPanel Create(RendererContext<Control> context)
    {
        return new WeiPoXAnimatedPanel();
    }

    protected override void Update(WeiPoXPanel control, Box widget)
    {
        base.Update(control, widget);
        if (widget is not PlatformAnimated platformAnimated || control is not WeiPoXAnimatedPanel animatedPanel)
        {
            return;
        }

        if (platformAnimated is { Initial: not null, Target: not null })
        {
            animatedPanel.SetTransitionData(platformAnimated.Initial, platformAnimated.Target, platformAnimated.Duration);
        }
        animatedPanel.SetDuration(platformAnimated.Duration);
    }
}

internal class WeiPoXAnimatedPanel : WeiPoXPanel
{
    // private CancellationTokenSource _cancellationTokenSource = new();
    public WeiPoXAnimatedPanel()
    {
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(new ScaleTransform());
        transformGroup.Children.Add(new TranslateTransform());
        RenderTransform = transformGroup;
        Transitions = new global::Avalonia.Animation.Transitions
        {
            new DoubleTransition
            {
                Property = OpacityProperty,
            },
            // TODO: not working
            new TransformOperationsTransition
            {
                Property = RenderTransformProperty,
            }
        };
    }

    private TransitionData? _initial;
    private TransitionData? _target;
    public void SetTransitionData(TransitionData initial, TransitionData target, TimeSpan duration)
    {
        if (initial != _initial || target != _target)
        {
            // if (_initial == null && _target == null)
            // {
            //     SetInitialState(target);
            // }
            // else
            // {
            //     _ = PlayTransition(initial, target, duration);
            // }
            PlayTransition(initial, target, duration);
        }
        _initial = initial;
        _target = target;
    }

    // private void SetInitialState(TransitionData target)
    // {
    //     Opacity = target.Fade?.Alpha ?? 1f;
    //     RenderTransformOrigin = new RelativePoint(target.Scale?.OriginX ?? 0.5, target.Scale?.OriginY ?? 0.5,
    //         RelativeUnit.Relative);
    //     var targetTransform = new TransformGroup();
    //     targetTransform.Children.Add(new ScaleTransform
    //     {
    //         ScaleX = target.Scale?.ScaleX ?? 1f,
    //         ScaleY = target.Scale?.ScaleY ?? 1f,
    //     });
    //     var targetSlideOffset = target.Slide?.SlideOffset.Invoke(new Offset(Bounds.Width, Bounds.Height)) ?? new Offset(0f, 0f);
    //     targetTransform.Children.Add(new TranslateTransform
    //     {
    //         X = targetSlideOffset.X,
    //         Y = targetSlideOffset.Y,
    //     });
    //     RenderTransform = targetTransform;
    // }

    private void PlayTransition(TransitionData initial, TransitionData target, TimeSpan duration)
    {
        Opacity = target.Fade?.Alpha ?? 1f;
        RenderTransformOrigin = new RelativePoint(target.Scale?.OriginX ?? 0.5, target.Scale?.OriginY ?? 0.5,
            RelativeUnit.Relative);
        if (RenderTransform is TransformGroup transformGroup)
        {
            if (transformGroup.Children.FirstOrDefault(x => x is ScaleTransform) is ScaleTransform scaleTransform)
            {
                scaleTransform.ScaleX = target.Scale?.ScaleX ?? 1f;
                scaleTransform.ScaleY = target.Scale?.ScaleY ?? 1f;
            }
        
            if (transformGroup.Children.FirstOrDefault(x => x is TranslateTransform) is TranslateTransform translateTransform)
            {
                var slideOffset = target.Slide?.SlideOffset.Invoke(new Offset(Bounds.Width, Bounds.Height)) ?? new Offset(0f, 0f);
                translateTransform.X = slideOffset.X;
                translateTransform.Y = slideOffset.Y;
            }
        }
        // var actualInitialOpacity = Opacity is 1d or 0d ? initial.Fade?.Alpha ?? 1f : Opacity;
        // _cancellationTokenSource.Cancel(false);
        // _cancellationTokenSource = new CancellationTokenSource();
        
        // var targetTransform = new TransformGroup();
        // targetTransform.Children.Add(new ScaleTransform
        // {
        //     ScaleX = target.Scale?.ScaleX ?? 1f,
        //     ScaleY = target.Scale?.ScaleY ?? 1f,
        // });
        // var targetSlideOffset = target.Slide?.SlideOffset.Invoke(new Offset(Bounds.Width, Bounds.Height)) ?? new Offset(0f, 0f);
        // targetTransform.Children.Add(new TranslateTransform
        // {
        //     X = targetSlideOffset.X,
        //     Y = targetSlideOffset.Y,
        // });
        //
        // var initialTransform = new TransformGroup();
        // initialTransform.Children.Add(new ScaleTransform
        // {
        //     ScaleX = initial.Scale?.ScaleX ?? 1f,
        //     ScaleY = initial.Scale?.ScaleY ?? 1f,
        // });
        // var initialSlideOffset = initial.Slide?.SlideOffset.Invoke(new Offset(Bounds.Width, Bounds.Height)) ?? new Offset(0f, 0f);
        // initialTransform.Children.Add(new TranslateTransform
        // {
        //     X = initialSlideOffset.X,
        //     Y = initialSlideOffset.Y,
        // });
        // var targetScale = $"scale({target.Scale?.ScaleX ?? 1f},{target.Scale?.ScaleY ?? 1f})";
        // var targetSlideOffset = target.Slide?.SlideOffset.Invoke(new Offset(Bounds.Width, Bounds.Height)) ?? new Offset(0f, 0f);
        // var targetTranslate = $"translate({targetSlideOffset.X}px,{targetSlideOffset.Y}px)";
        // var targetTransform = TransformOperations.Parse($"{targetScale} {targetTranslate}");
        // var initialScale = $"scale({initial.Scale?.ScaleX ?? 1f},{initial.Scale?.ScaleY ?? 1f})";
        // var initialSlideOffset = initial.Slide?.SlideOffset.Invoke(new Offset(Bounds.Width, Bounds.Height)) ?? new Offset(0f, 0f);
        // var initialTranslate = $"translate({initialSlideOffset.X}px,{initialSlideOffset.Y}px)";
        // var initialTransform = TransformOperations.Parse($"{initialScale} {initialTranslate}");
        //
        // var initialTransformSetter = new Setter(RenderTransformProperty, initialTransform);
        // var initialOpacitySetter = new Setter(OpacityProperty, Convert.ToDouble(actualInitialOpacity));
        // var targetTransformSetter = new Setter(RenderTransformProperty, targetTransform);
        // var targetOpacitySetter = new Setter(OpacityProperty, Convert.ToDouble(target.Fade?.Alpha ?? 1f));
        // global::Avalonia.Animation.Animation.SetAnimator(initialTransformSetter, typeof(TransformOperationsAnimator));
        // global::Avalonia.Animation.Animation.SetAnimator(targetTransformSetter, typeof(TransformOperationsAnimator));
        // await new global::Avalonia.Animation.Animation
        // {
        //     Duration = duration,
        //     FillMode = FillMode.None,
        //     Children =
        //     {
        //         new KeyFrame
        //         {
        //             Cue = new Cue(0),
        //             Setters =
        //             {
        //                 initialOpacitySetter,
        //                 initialTransformSetter,
        //             }
        //         },
        //         new KeyFrame
        //         {
        //             Cue = new Cue(1),
        //             Setters =
        //             {
        //                 targetOpacitySetter,
        //                 targetTransformSetter,
        //             }
        //         }
        //     },
        // }.RunAsync(this, _cancellationTokenSource.Token);
        // if (_cancellationTokenSource.IsCancellationRequested)
        // {
        //     return;
        // }
        // Opacity = target.Fade?.Alpha ?? 1f;
        // this.RenderTransform = targetTransform;
    }

    public void SetDuration(TimeSpan platformAnimatedDuration)
    {
        if (Transitions == null)
        {
            return;
        }
        foreach (var transition in Transitions)
        {
            switch (transition)
            {
                case DoubleTransition doubleTransition:
                    doubleTransition.Duration = platformAnimatedDuration;
                    break;
                case TransformOperationsTransition transformOperationsTransition:
                    transformOperationsTransition.Duration = platformAnimatedDuration;
                    break;
            }
        }
    }
}