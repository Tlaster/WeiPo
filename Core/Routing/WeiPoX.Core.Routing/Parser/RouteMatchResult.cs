using System.Collections.Immutable;

namespace WeiPoX.Core.Routing.Parser;

internal record RouteMatchResult(Route Route, ImmutableDictionary<string, string> PathMap);