using System.Collections;
using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Widgets.Layout;

public abstract record LazyLayout : MappingWidget, ILazyWidget, IEnumerable
{
    private readonly List<LazyItem> _content = new();
    public IEnumerator GetEnumerator()
    {
        return _content.GetEnumerator();
    }
    
    public void Add(LazyItem item)
    {
        _content.Add(item);
    }

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

        return base.Equals(other) && _content.SequenceEqual(other._content);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _content);
    }

    public int Count => _content.Sum(it => it switch
    {
        Item _ => 1,
        Items items => items.Count,
        _ => throw new ArgumentOutOfRangeException(nameof(it))
    });
    
    public Func<Widget>? GetBuilder(int index)
    {
        var count = 0;
        foreach (var item in _content)
        {
            switch (item)
            {
                case Item _:
                    if (count == index)
                    {
                        return () => ((Item) item).Content;
                    }

                    count++;
                    break;
                case Items items:
                    if (count + items.Count > index)
                    {
                        return () => items.Builder.Invoke(index - count);
                    }

                    count += items.Count;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item));
            }
        }

        return null;
    }

    public Func<Range>? GetVisibleRange { get; set; }
}


public abstract record LazyItem;
public record Item : LazyItem, IEnumerable
{
    public Widget Content { get; private set; } = new Box();
    public void Add(Widget widget)
    {
        Content = widget;
    }

    public IEnumerator GetEnumerator()
    {
        return new[] {Content}.GetEnumerator();
    }
}

public record Items(int Count) : LazyItem
{
    public required Func<int, Widget> Builder { get; init; }
}