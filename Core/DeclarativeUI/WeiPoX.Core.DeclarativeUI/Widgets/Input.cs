namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record Input : MappingWidget
{
    public required InputState State { get; init; }
    public required Action<InputState> OnStateChanged { get; init; }
}

public record InputState(string Text, int SelectionStart = 0, int SelectionEnd = 0);