using System.Collections.Immutable;
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
    public required bool Visible { get; init; }
    public required Widget Child { get; init; }
    public EnterTransition EnterTransition { get; init; } = new FadeIn();
    public ExitTransition ExitTransition { get; init; } = new FadeOut();
    protected override Widget Build()
    {
        
    }
}

internal record PlatformAnimated : Box
{
    public required TransitionData TransitionData { get; init; }
}

internal record TransitionData(Fade? Fade = null, Scale? Scale = null, Slide? Slide = null, ChangeSize? ChangeSize = null, TimeSpan Duration = default);
internal record Fade(float Alpha);
internal record Scale(float ScaleX, float ScaleY, float OriginX, float OriginY);
internal delegate Size ChangeSizeDelegate(Size fullSize);
internal record ChangeSize(ChangeSizeDelegate Size);
internal delegate Offset ChangePositionDelegate(Offset fullOffset);
internal record Slide(ChangePositionDelegate SlideOffset);

public abstract record EnterTransition
{
    internal EnterTransition(TransitionData transitionData)
    {
        TransitionData = transitionData;
    }

    internal TransitionData TransitionData { get; }
}

public abstract record ExitTransition
{
    internal ExitTransition(TransitionData transitionData)
    {
        TransitionData = transitionData;
    }

    internal TransitionData TransitionData { get; }
}

public record FadeIn : EnterTransition
{
    public FadeIn(float initialAlpha = 0f) : this(initialAlpha, TimeSpan.FromMilliseconds(300))
    {
    }
    
    public FadeIn(float initialAlpha, TimeSpan duration = default) : base(new TransitionData(Fade: new Fade(initialAlpha), Duration: duration))
    {
    }
}

public record FadeOut : ExitTransition
{
    public FadeOut(float finalAlpha = 0f) : this(finalAlpha, TimeSpan.FromMilliseconds(300))
    {
    }
    
    public FadeOut(float finalAlpha, TimeSpan duration = default) : base(new TransitionData(Fade: new Fade(finalAlpha), Duration: duration))
    {
    }
}
