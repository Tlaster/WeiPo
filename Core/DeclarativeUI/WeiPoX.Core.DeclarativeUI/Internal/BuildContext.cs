using System.Collections.Immutable;

namespace WeiPoX.Core.DeclarativeUI.Internal;

internal record BuildContext(ImmutableDictionary<Type, object> ContextMap)
{
    public static BuildContext operator +(BuildContext context, (Type type, object value) pair)
    {
        return new BuildContext(context.ContextMap.SetItem(pair.type, pair.value));
    }

    public static BuildContext operator +(BuildContext context, BuildContext other)
    {
        return new BuildContext(context.ContextMap.AddRange(other.ContextMap));
    }

    public static BuildContext operator +(BuildContext context, IEnumerable<KeyValuePair<Type, object>> pairs)
    {
        return new BuildContext(context.ContextMap.AddRange(pairs));
    }

    public static BuildContext operator -(BuildContext context, Type type)
    {
        return new BuildContext(context.ContextMap.Remove(type));
    }

    public static BuildContext operator -(BuildContext context, BuildContext other)
    {
        return new BuildContext(context.ContextMap.RemoveRange(other.ContextMap.Keys));
    }

    public static BuildContext operator -(BuildContext context, IEnumerable<Type> types)
    {
        return new BuildContext(context.ContextMap.RemoveRange(types));
    }

    public T? Get<T>()
    {
        return ContextMap.TryGetValue(typeof(T), out var value) ? (T)value : default;
    }
}