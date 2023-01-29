using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI;

public static class StatefulWidgetExtensions
{
    public static T UseObservable<T>(this StatefulWidget statefulWidget, IObservable<T> observable, T initialValue)
    {
        var state = statefulWidget.UseState(initialValue);
        statefulWidget.UseEffect(() =>
        {
            var subscription = observable.Subscribe(state.SetValue);
            return subscription.Dispose;
        }, observable);
        return state.Value;
    }
}