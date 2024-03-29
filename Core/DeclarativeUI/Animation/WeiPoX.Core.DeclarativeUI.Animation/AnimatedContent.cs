﻿using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Animation;

public record AnimatedContent<T> : StatefulWidget where T : notnull
{
    public required T State { get; init; }
    public required Func<T, Widget> Builder { get; init; }
    // TODO: add animation
    
    protected override Widget Build()
    {
        var (currentlyVisible, setCurrentlyVisible) = UseState(() => ImmutableList.Create(State));
        var (contentMap, setContentMap) = UseState(ImmutableDictionary<T, Widget>.Empty);
        var (currentState, setCurrentState) = UseState(State);
        var (targetState, setTargetState) = UseState(State);

        UseEffect(() =>
        {
            if (!targetState.Equals(State))
            {
                setTargetState(State);
            }
        }, State);
        
        // TODO: set current state

        if (!currentlyVisible.Contains(targetState))
        {
            setCurrentlyVisible(currentlyVisible.Add(targetState));
        }

        if (currentState.Equals(targetState))
        {
            if (currentlyVisible.Count != 1 || !currentlyVisible[0].Equals(currentState))
            {
                setCurrentlyVisible(ImmutableList.Create(currentState));
            }

            if (contentMap.Count != 1 || contentMap.ContainsKey(currentState))
            {
                setContentMap(ImmutableDictionary<T, Widget>.Empty);
            }
        }
        
        if (!currentState.Equals(targetState) && !currentlyVisible.Contains(targetState))
        {
            var index = currentlyVisible.IndexOf(targetState);
            setCurrentlyVisible(index == -1
                ? currentlyVisible.Add(targetState)
                : currentlyVisible.RemoveAt(index).Add(targetState));
        }

        if (!contentMap.ContainsKey(targetState) || !contentMap.ContainsKey(currentState))
        {
            setContentMap(
                currentlyVisible
                    .Select(it =>
                    {
                        // TODO: animation controller
                        return new KeyValuePair<T, Widget>(it, Builder(it));
                    })
                    .ToImmutableDictionary()
            );
        }


        return new Box
        {
            Children = currentlyVisible.Select(x => contentMap[x]).ToList(),
        };
    }
}

public record AnimatedVisibility : StatefulWidget
{
    public bool ClipBounds { get; init; } = false;
    public required bool Visible { get; init; }
    public required Widget Child { get; init; }
    public TimeSpan Duration { get; init; } = Transitions.DefaultDuration;
    public EnterTransition EnterTransition { get; init; } = Transitions.FadeIn() + Transitions.SlideInFromTop();
    public ExitTransition ExitTransition { get; init; } = Transitions.FadeOut() + Transitions.SlideOutToTop();
    protected override Widget Build()
    {
        return new Box
        {
            ClipBounds = ClipBounds,
            Children =
            {
                new PlatformAnimated
                {
                    Children = 
                    {
                        Child,
                    },
                    Duration = Duration,
                    Target = Visible ? TransitionData.Empty : ExitTransition.TransitionData,
                    Initial = Visible ? EnterTransition.TransitionData : TransitionData.Empty,
                }
            }
        };
    }
}

public record PlatformAnimated : Box
{
    internal TimeSpan Duration { get; init; }
    internal TransitionData? Target { get; init; }
    internal TransitionData? Initial { get; init; }
}

internal record TransitionData(Fade? Fade = null, Scale? Scale = null, Slide? Slide = null,
    ChangeSize? ChangeSize = null)
{
    public static TransitionData Empty { get; } = new(new Fade(1f), new Scale(1f, 1f, 0.5f, 0.5f),
        new Slide(_ => new Offset(0f, 0f)), new ChangeSize(_ => new Size(0f, 0f)));
}

internal record Fade(float Alpha);
internal record Scale(float ScaleX, float ScaleY, float OriginX, float OriginY);
public delegate Size ChangeSizeDelegate(Size fullSize);
internal record ChangeSize(ChangeSizeDelegate Size);
public delegate Offset ChangePositionDelegate(Offset fullOffset);
internal record Slide(ChangePositionDelegate SlideOffset);

public static class Transitions
{
    public static readonly TimeSpan DefaultDuration = TimeSpan.FromMilliseconds(3000);
    public static EnterTransition FadeIn(float initialAlpha = 0f) => new EnterTransitionImpl(new TransitionData(Fade: new Fade(initialAlpha)));
    public static ExitTransition FadeOut(float finalAlpha = 0f) => new ExitTransitionImpl(new TransitionData(Fade: new Fade(finalAlpha)));
    public static EnterTransition ScaleIn(float initialScaleX = 0f, float initialScaleY = 0f, float originX = 0.5f, float originY = 0.5f) =>
        new EnterTransitionImpl(new TransitionData(Scale: new Scale(initialScaleX, initialScaleY, originX, originY)));
    public static ExitTransition ScaleOut(float finalScaleX = 0f, float finalScaleY = 0f, float originX = 0.5f, float originY = 0.5f) =>
        new ExitTransitionImpl(new TransitionData(Scale: new Scale(finalScaleX, finalScaleY, originX, originY)));
    public static EnterTransition SlideIn(ChangePositionDelegate slideOffset) => new EnterTransitionImpl(new TransitionData(Slide: new Slide(slideOffset)));
    public static EnterTransition SlideInFromStart() => SlideIn(fullSize => new Offset(-fullSize.X, 0f));
    public static EnterTransition SlideInFromEnd() => SlideIn(fullSize => new Offset(fullSize.X, 0f));
    public static EnterTransition SlideInFromTop() => SlideIn(fullSize => new Offset(0f, -fullSize.Y));
    public static EnterTransition SlideInFromBottom() => SlideIn(fullSize => new Offset(0f, fullSize.Y));
    public static ExitTransition SlideOut(ChangePositionDelegate slideOffset) => new ExitTransitionImpl(new TransitionData(Slide: new Slide(slideOffset)));
    public static ExitTransition SlideOutToStart() => SlideOut(fullSize => new Offset(-fullSize.X, 0f));
    public static ExitTransition SlideOutToEnd() => SlideOut(fullSize => new Offset(fullSize.X, 0f));
    public static ExitTransition SlideOutToTop() => SlideOut(fullSize => new Offset(0f, -fullSize.Y));
    public static ExitTransition SlideOutToBottom() => SlideOut(fullSize => new Offset(0f, fullSize.Y));
}


public abstract record EnterTransition
{
    internal EnterTransition(TransitionData transitionData)
    {
        TransitionData = transitionData;
    }

    internal TransitionData TransitionData { get; }
    
    // override plus operator
    public static EnterTransition operator +(EnterTransition a, EnterTransition b) =>
        new EnterTransitionImpl(
            new TransitionData(
                Fade: a.TransitionData.Fade ?? b.TransitionData.Fade,
                Scale: a.TransitionData.Scale ?? b.TransitionData.Scale,
                Slide: a.TransitionData.Slide ?? b.TransitionData.Slide,
                ChangeSize: a.TransitionData.ChangeSize ?? b.TransitionData.ChangeSize
            )
        );
}

internal record EnterTransitionImpl : EnterTransition
{
    public EnterTransitionImpl(TransitionData transitionData) : base(transitionData)
    {
    }
}

public abstract record ExitTransition
{
    internal ExitTransition(TransitionData transitionData)
    {
        TransitionData = transitionData;
    }

    internal TransitionData TransitionData { get; }
    
    // override plus operator
    public static ExitTransition operator +(ExitTransition a, ExitTransition b) =>
        new ExitTransitionImpl(
            new TransitionData(
                Fade: a.TransitionData.Fade ?? b.TransitionData.Fade,
                Scale: a.TransitionData.Scale ?? b.TransitionData.Scale,
                Slide: a.TransitionData.Slide ?? b.TransitionData.Slide,
                ChangeSize: a.TransitionData.ChangeSize ?? b.TransitionData.ChangeSize
            )
        );
}

internal record ExitTransitionImpl : ExitTransition
{
    public ExitTransitionImpl(TransitionData transitionData) : base(transitionData)
    {
    }
}