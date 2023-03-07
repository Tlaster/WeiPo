namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record Input(InputState State, Action<InputState> OnStateChanged) : MappingWidget;

public record InputState(string Text, int SelectionStart = 0, int SelectionEnd = 0);