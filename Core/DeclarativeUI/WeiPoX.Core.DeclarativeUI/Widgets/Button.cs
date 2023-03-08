using System.Collections.Immutable;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Widgets;

public record Button : MappingWidget
{
    public required string Text { get; init; }
    public required Action OnClick { get; init; }
}