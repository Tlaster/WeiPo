using System.Collections;
using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public abstract record LazyLayout : MappingWidget, ILazyWidget, IEnumerable
{
    ImmutableList<ActualLazyItem> ILazyWidget.Items => GenerateActualLazyItems().ToImmutableList();
    public List<LazyItem> Content { get; } = new();
    public IEnumerator GetEnumerator()
    {
        return Content.GetEnumerator();
    }
    
    public void Add(LazyItem item)
    {
        Content.Add(item);
    }
    
    internal List<ActualLazyItem> GenerateActualLazyItems() =>
        Content.SelectMany(it =>
        {
            return it switch
            {
                Item item => new [] { new Func<Widget>(() => item.Content)}.ToList(),
                Items items => Enumerable.Range(0, items.Count).Select(index => new Func<Widget>(() => items.Builder.Invoke(index))).ToList(),
                _ => throw new ArgumentOutOfRangeException(nameof(it))
            };
        }).Select((builder, index) => new ActualLazyItem(index, builder)).ToList();

    public virtual bool Equals(LazyLayout? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && Content.SequenceEqual(other.Content);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Content);
    }
}


public abstract record LazyItem;
public record Item : LazyItem
{
    public required Widget Content { get; init; }
}

public record Items(int Count) : LazyItem
{
    public required Func<int, Widget> Builder { get; init; }
}