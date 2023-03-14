using System.Collections;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public record LazyColumn : MappingWidget, IEnumerable
{
    public List<ILazyItem> Items { get; } = new();
    public IEnumerator GetEnumerator()
    {
        return Items.GetEnumerator();
    }
    
    public void Add(ILazyItem item)
    {
        Items.Add(item);
    }
    
    internal List<ActualLazyItem> GenerateActualLazyItems() =>
        Items.SelectMany(it =>
        {
            return it switch
            {
                Item item => new [] { new Func<Widget>(() => item.Content)}.ToList(),
                Items items => Enumerable.Range(0, items.Count).Select(index => new Func<Widget>(() => items.Builder.Invoke(index))).ToList(),
                _ => throw new ArgumentOutOfRangeException(nameof(it))
            };
        }).Select((builder, index) => new ActualLazyItem(index, builder)).ToList();

    public virtual bool Equals(LazyColumn? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && Items.SequenceEqual(other.Items);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Items);
    }
}

internal record ActualLazyItem(int Index, Func<Widget> Builder);

public interface ILazyItem
{
    
}

public record Item : ILazyItem
{
    public required Widget Content { get; init; }
}

public record Items(int Count) : ILazyItem
{
    public required Func<int, Widget> Builder { get; init; }
}