using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace WeiPoX.Core.Routing;

public record QueryString
{
    internal QueryString(string raw)
    {
        Value = raw.Split('?')
                    .LastOrDefault()
                    ?.Split('&')
                    .Select(x => x.Split('='))
                    .Where(it => !string.IsNullOrEmpty(it.FirstOrDefault()))
                    .Where(it => it.Length is 2 or 1)
                    .Select(it => (it[0], it.ElementAtOrDefault(1)))
                    .GroupBy(it => it.Item1)
                    .Select(it => (it.Key,
                        it.Where(x => !string.IsNullOrEmpty(x.Item2)).Select(x => x.Item2!).ToImmutableList()))
                    .ToList()
                    .ToImmutableDictionary(it => it.Key, it => it.Item2) ??
                ImmutableDictionary<string, ImmutableList<string>>.Empty;
    }

    public ImmutableDictionary<string, ImmutableList<string>> Value { get; }
}