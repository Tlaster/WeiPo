using System.Collections.Immutable;
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