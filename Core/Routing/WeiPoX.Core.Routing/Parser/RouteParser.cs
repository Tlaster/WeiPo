using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace WeiPoX.Core.Routing.Parser;

internal class RouteParser
{
    private const int NtStatic = 0; // /home
    private const int NtRegexp = 1; // /{id:[0-9]+}
    private const int NtParam = 2; // /{user}
    private const int NtCatchAll = 3; // /api/v1/*
    private const int NodeSize = NtCatchAll + 1;
    private const char ZeroChar = '\0';
    private const string BASE_CATCH_ALL = "/?*";

    private readonly Node _root = new();
    private readonly Dictionary<string, Route> _staticPaths = new();

    public void Insert(string pattern, Route route)
    {
        var patternStr = pattern;
        var baseCatchAll = BaseCatchAll(patternStr);
        if (baseCatchAll.Length > 1)
        {
            // Add route pattern: /static/?* => /static
            Insert(baseCatchAll, route);
            var tail = patternStr[(baseCatchAll.Length + 2)..];
            patternStr = $"{baseCatchAll}/{tail}";
        }

        if (patternStr == BASE_CATCH_ALL)
        {
            patternStr = "/*";
        }

        if (!PathKeys(patternStr).Any())
        {
            _staticPaths[patternStr] = route;
        }

        _root.InsertRoute(patternStr, route);
    }

    private string BaseCatchAll(string pattern)
    {
        var i = pattern.IndexOf(BASE_CATCH_ALL, StringComparison.Ordinal);
        return i > 0 ? pattern[..i] : "";
    }

    public void Insert(Route route)
    {
        Insert(route.Path, route);
    }

    public RouteMatchResult? Find(string path)
    {
        var staticRoute = _staticPaths.GetValueOrDefault(path);
        return staticRoute == null
            ? FindInternal(path)
            : new RouteMatchResult(staticRoute,  ImmutableDictionary<string, string>.Empty);
    }

    private RouteMatchResult? FindInternal(string path)
    {
        // use radix tree
        var result = new RouteMatch();
        var route = _root.FindRoute(result, path);
        return route == null ? null : new RouteMatchResult(route, result.PathMap.ToImmutableDictionary());
    }

    public static List<string> PathKeys(string pattern, Action<string, string?>? onItem = null)
    {
        var result = new List<string>();
        var start = -1;
        var end = int.MaxValue;
        var len = pattern.Length;
        var curly = 0;
        var i = 0;
        while (i < len)
        {
            var ch = pattern[i];
            if (ch == '{')
            {
                if (curly == 0)
                {
                    start = i + 1;
                    end = int.MaxValue;
                }

                curly += 1;
            }
            else if (ch == ':')
            {
                end = i;
            }
            else if (ch == '}')
            {
                curly -= 1;
                if (curly == 0)
                {
                    var id = pattern[start..Math.Min(i, end)];
                    var value = end == int.MaxValue ? null : pattern[(end + 1)..i];
                    onItem?.Invoke(id, value);
                    result.Add(id);
                    start = -1;
                    end = int.MaxValue;
                }
            }
            else if (ch == '*')
            {
                var id = i == len - 1 ? "*" : pattern[(i + 1)..];
                onItem?.Invoke(id, "\\.*");
                result.Add(id);
                i = len;
            }

            i++;
        }

        return result;
    }

    public static List<string> ExpandOptionalVariables(string pattern)
    {
        if (string.IsNullOrEmpty(pattern) || pattern == "/")
        {
            return new List<string>
            {
                "/"
            };
        }

        var len = pattern.Length;
        var key = 0;
        var paths = new Dictionary<int, StringBuilder>();

        void PathAppender(int index, StringBuilder segment)
        {
            for (var i = index; i < index; i++)
            {
                paths[i].Append(segment);
            }

            if (!paths.ContainsKey(index))
            {
                var value = new StringBuilder();
                if (index > 0)
                {
                    var prev = paths[index - 1];
                    if (prev.ToString() != "/")
                    {
                        value.Append(prev);
                    }
                }
                paths[index] = value;
            }
            paths[index].Append(segment);
        }

        var segment = new StringBuilder();
        var isLastOptional = false;
        var i = 0;
        while (i < len)
        {
            var ch = pattern[i];
            if (ch == '/')
            {
                if (segment.Length > 0)
                {
                    PathAppender(key, segment);
                    segment.Clear();
                }

                segment.Append(ch);
                i += 1;
            } 
            else if (ch == '{')
            {
                segment.Append(ch);
                var curly = 1;
                var j = i + 1;
                while (j < len)
                {
                    var next = pattern[j++];
                    segment.Append(next);
                    if (next == '{')
                    {
                        curly += 1;
                    } 
                    else if (next == '}')
                    {
                        curly -= 1;
                        if (curly == 0)
                        {
                            break;
                        }
                    }
                }

                if (j < len && pattern[j] == '?')
                {
                    j += 1;
                    isLastOptional = true;
                    if (paths.Count == 0)
                    {
                        paths[0] = new StringBuilder("/");
                    }
                    PathAppender(++key, segment);
                }
                else
                {
                    isLastOptional = false;
                    PathAppender(key, segment);
                }
                segment.Clear();
                i = j;
            }
            else
            {
                segment.Append(ch);
                i += 1;
            }
        }

        if (paths.Count == 0)
        {
            return new List<string>
            {
                pattern
            };
        }

        if (segment.Length > 0)
        {
            PathAppender(key, segment);
            if (isLastOptional)
            {
                paths[++key] = segment;
            }
        }

        return paths.Values.Select(it => it.ToString()).ToList();
    }
    

    private record Segment(int NodeType = 0, string RexPat = "", char Tail = '\0', int StartIndex = 0,
        int EndIndex = 0);

    private record Node(int Type = 0, string Prefix = "", char Label = '\0', char Tail = '\0', Regex? Rex = null,
        string? ParamsKey = null, Route? Route = null) : IComparable<Node>
    {
        public char Label { get; private set; } = Label;
        public string Prefix { get; private set; } = Prefix;
        public Regex? Rex { get; private set; } = Rex;
        public int Type { get; private set; } = Type;
        public char Tail { get; private set; } = Tail;
        public string? ParamsKey { get; private set; } = ParamsKey;

        public Route? Route { get; private set; } = Route;

        // subroutes on the leaf node
        // Routes subroutes;
        // child nodes should be stored in-order for iteration,
        // in groups of the node type.
        public Dictionary<int, List<Node>> Children { get; } = new();

        public bool IsLeaf => Route != null;

        public int CompareTo(Node? other)
        {
            if (other is null)
            {
                return 1;
            }

            return Label - other.Label;
        }

        public Node InsertRoute(string pattern, Route route)
        {
            var n = this;
            Node parent;
            var search = pattern;
            while (true)
            {
                // Handle key exhaustion
                if (string.IsNullOrEmpty(search))
                {
                    // Insert or update the node's leaf handler
                    n.ApplyRoute(route);
                    return n;
                }

                // We're going to be searching for a wild node next,
                // in this case, we need to get the tail
                var label = search[0];
                //        char segTail;
                //        int segEndIdx;
                //        int segTyp;
                var seg = label is '{' or '*' ? PatNextSegment(search) : new Segment();

                var prefix = seg.NodeType == NtRegexp ? seg.RexPat : "";

                // Look for the edge to attach to
                parent = n;
                n = n.GetEdge(seg.NodeType, label, seg.Tail, prefix);
                if (n == null)
                {
                    var nchild = new Node(Label: label, Tail: seg.Tail, Prefix: search);
                    var nhn = parent.AddChild(nchild, search);
                    nhn.ApplyRoute(route);
                    return nhn;
                }

                // Found an edge to newRuntimeRoute the pattern
                if (n.Type > NtStatic)
                {
                    // We found a param node, trim the param from the search path and continue.
                    // This param/wild pattern segment would already be on the tree from a previous
                    // call to addChild when creating a new node.
                    search = search[seg.EndIndex..];
                    continue;
                }

                // Static nodes fall below here.
                // Determine longest prefix of the search key on newRuntimeRoute.
                var commonPrefix = LongestPrefix(search, n.Prefix);
                if (commonPrefix == n.Prefix.Length)
                {
                    // the common prefix is as long as the current node's prefix we're attempting to insert.
                    // keep the search going.
                    search = search[commonPrefix..];
                    continue;
                }

                // Split the node
                var child = new Node(NtStatic, search[..commonPrefix]);
                parent.ReplaceChild(search[0], seg.Tail, child);

                // Restore the existing node
                n.Label = n.Prefix[commonPrefix];
                n.Prefix = n.Prefix[commonPrefix..];
                child.AddChild(n, n.Prefix);

                // If the new key is a subset, set the route on this node and finish.
                search = search[commonPrefix..];
                if (string.IsNullOrEmpty(search))
                {
                    child.ApplyRoute(route);
                    return child;
                }

                // Create a new edge for the node
                var subchild = new Node(NtStatic, Label: search[0], Prefix: search);
                var hn = child.AddChild(subchild, search);
                hn.ApplyRoute(route);
                return hn;
            }
        }

        // addChild appends the new `child` node to the tree using the `pattern` as the trie key.
        // For a URL router like chi's, we split the static, param, regexp and wildcard segments
        // into different nodes. In addition, addChild will recursively call itself until every
        // pattern segment is added to the url pattern tree as individual nodes, depending on type.
        public Node AddChild(Node child, string search)
        {
            var searchStr = search;
            var n = this;
            //      String search = prefix.toString();

            // handler leaf node added to the tree is the child.
            // this may be overridden later down the flow
            var hn = child;

            // Parse next segment
            //      segTyp, _, segRexpat, segTail, segStartIdx, segEndIdx := patNextSegment(search)
            var seg = PatNextSegment(searchStr);
            var segTyp = seg.NodeType;
            var segStartIdx = seg.StartIndex;
            var segEndIdx = seg.EndIndex;
            switch (segTyp)
            {
                case NtStatic:
                    break;
                default:
                    // Search prefix contains a param, regexp or wildcard
                    if (segTyp == NtRegexp)
                    {
                        child.Prefix = seg.RexPat;
                        child.Rex = new Regex(seg.RexPat);
                    }

                    if (segStartIdx == 0)
                    {
                        // Route starts with a param
                        child.Type = segTyp;
                        if (segTyp == NtCatchAll)
                        {
                            segStartIdx = -1;
                        }
                        else
                        {
                            segStartIdx = segEndIdx;
                        }

                        if (segStartIdx < 0)
                        {
                            segStartIdx = searchStr.Length;
                        }

                        child.Tail = seg.Tail; // for params, we set the tail
                        child.ParamsKey = FindParamKey(searchStr); // set paramsKey if it has keys
                        if (segStartIdx != searchStr.Length)
                        {
                            // add static edge for the remaining part, split the end.
                            // its not possible to have adjacent param nodes, so its certainly
                            // going to be a static node next.
                            searchStr = searchStr[segStartIdx..]; // advance search position
                            var nn = new Node(NtStatic, Label: searchStr[0], Prefix: searchStr);
                            hn = child.AddChild(nn, searchStr);
                        }
                    }
                    else if (segStartIdx > 0)
                    {
                        // Route has some param

                        // starts with a static segment
                        child.Type = NtStatic;
                        child.Prefix = searchStr[..segStartIdx];
                        child.Rex = null;

                        // add the param edge node
                        searchStr = searchStr[segStartIdx..];
                        var nn = new Node(segTyp, Label: searchStr[0], Tail: seg.Tail,
                            ParamsKey: FindParamKey(searchStr));
                        hn = child.AddChild(nn, searchStr);
                    }

                    break;
            }

            var result = Append(n.Children.GetValueOrDefault(child.Type), child);
            TailSort(result);
            n.Children[child.Type] = result;
            return hn;
        }

        private string? FindParamKey(string pattern)
        {
            const string startChar = "{";
            const string endChar = "}";
            const string regexStart = ":";
            if (!pattern.StartsWith("{") && !pattern.EndsWith("}"))
            {
                return null;
            }

            var startIndex = pattern.IndexOf(startChar, StringComparison.Ordinal);
            var endIndex = pattern.IndexOf(endChar, StringComparison.Ordinal);
            var regexIndex = pattern.IndexOf(regexStart, StringComparison.Ordinal);
            if (regexIndex == -1)
            {
                regexIndex = pattern.Length;
            }

            return pattern[Math.Min(startIndex + 1, pattern.Length - 1)..Math.Min(endIndex, regexIndex)];
        }

        public void ReplaceChild(char label, char tail, Node child)
        {
            var n = this;
            var children = n.Children.GetValueOrDefault(child.Type);
            if (children == null)
            {
                return;
            }

            var i = 0;
            while (i < children.Count)
            {
                if (children[i].Label == label && children[i].Tail == tail)
                {
                    children[i] = child;
                    children[i].Label = label;
                    children[i].Tail = tail;
                    return;
                }

                i++;
            }

            throw new ArgumentException("chi: replacing missing child");
        }

        public Node? GetEdge(int ntyp, char label, char tail, string prefix)
        {
            var n = this;
            var nds = n.Children.GetValueOrDefault(ntyp);
            if (nds == null)
            {
                return null;
            }

            var i = 0;
            while (i < nds.Count)
            {
                if (nds[i].Label == label && nds[i].Tail == tail)
                {
                    if (ntyp == NtRegexp && nds[i].Prefix != prefix)
                    {
                        i++;
                        continue;
                    }

                    return nds[i];
                }

                i++;
            }

            return null;
        }

        public void ApplyRoute(Route? route)
        {
            var n = this;
            n.Route = route;
        }

        public Route? FindRoute(RouteMatch rctx, string path)
        {
            for (var ntyp = 0; ntyp < NodeSize; ntyp++)
            {
                var nds = Children.GetValueOrDefault(ntyp);
                if (nds == null)
                {
                    continue;
                }

                Node? xn = null;
                var xsearch = path;
                var label = !string.IsNullOrEmpty(path) ? path[0] : ZeroChar;

                switch (ntyp)
                {
                    case NtStatic:
                        xn = FindEdge(nds, label);
                        if (xn == null || !xsearch.StartsWith(xn.Prefix))
                        {
                            continue;
                        }

                        xsearch = xsearch[xn.Prefix.Length..];
                        break;
                    case NtParam:
                    case NtRegexp:
                        // short-circuit and return no matching route for empty param values
                        if (string.IsNullOrEmpty(xsearch))
                        {
                            continue;
                        }

                        // serially loop through each node grouped by the tail delimiter
                        var idx = 0;
                        while (idx < nds.Count)
                        {
                            xn = nds[idx];
                            if (xn.Type != NtStatic)
                            {
                                if (xn.ParamsKey != null)
                                {
                                    rctx.Keys.Add(xn.ParamsKey);
                                }
                            }

                            // label for param nodes is the delimiter byte
                            var p = xsearch.IndexOf(xn.Tail);
                            if (p < 0)
                            {
                                if (xn.Tail == '/')
                                {
                                    p = xsearch.Length;
                                }
                                else
                                {
                                    // TODO: Check
                                    // idx++;
                                    p = idx++;
                                    continue;
                                }
                            }

                            var rex = xn.Rex;
                            if (ntyp == NtRegexp && rex != null)
                            {
                                if (!rex.IsMatch(xsearch[..p]))
                                {
                                    idx++;
                                    continue;
                                }
                            }
                            else if (xsearch[..p].IndexOf('/') != -1)
                            {
                                // avoid a newRuntimeRoute across path segments
                                idx++;
                                continue;
                            }

                            // rctx.routeParams.Values = append(rctx.routeParams.Values, xsearch[:p])
                            var prevlen = rctx.Vars.Count;
                            rctx.Value(xsearch[..p]);
                            xsearch = xsearch[p..];
                            if (xsearch.Length == 0)
                            {
                                if (xn.IsLeaf)
                                {
                                    var h = xn.Route;
                                    if (h != null)
                                    {
                                        rctx.Key();
                                        return h;
                                    }
                                }
                            }

                            // recursively find the next node on this branch
                            var route = xn.FindRoute(rctx, xsearch);
                            if (route != null)
                            {
                                return route;
                            }

                            // not found on this branch, reset vars
                            rctx.Truncate(prevlen);
                            xsearch = path;
                            idx++;
                        }

                        break;
                    default:
                        // catch-all nodes
                        // rctx.routeParams.Values = append(rctx.routeParams.Values, search)
                        if (xsearch.Any())
                        {
                            rctx.Value(xsearch);
                        }

                        xn = nds[0];
                        xsearch = "";
                        break;
                }

                if (xn == null)
                {
                    continue;
                }

                // did we returnType it yet?
                if (xsearch.Length == 0)
                {
                    if (xn.IsLeaf)
                    {
                        var h = xn.Route;
                        if (h != null)
                        {
                            // rctx.routeParams.Keys = append(rctx.routeParams.Keys, h.paramKeys...)
                            rctx.Key();
                            return h;
                        }
                    }
                }

                // recursively returnType the next node..
                var fin = xn.FindRoute(rctx, xsearch);
                if (fin != null)
                {
                    return fin;
                }

                // Did not returnType final handler, let's remove the param here if it was set
                if (xn.Type > NtStatic)
                {
                    //          if len(rctx.routeParams.Values) > 0 {
                    //            rctx.routeParams.Values = rctx.routeParams.Values[:len(rctx.routeParams.Values) - 1]
                    //          }
                    rctx.Pop();
                }
            }

            return null;
        }

        public Node? FindEdge(List<Node> ns, char label)
        {
            var num = ns.Count;
            var idx = 0;
            var i = 0;
            var j = num - 1;
            while (i <= j)
            {
                idx = i + (j - i) / 2;
                if (label > ns[idx].Label)
                {
                    i = idx + 1;
                }
                else if (label < ns[idx].Label)
                {
                    j = idx - 1;
                }
                else
                {
                    i = num; // breaks cond
                }
            }

            return ns[idx].Label != label ? null : ns[idx];
        }

        public int LongestPrefix(string k1, string k2)
        {
            var len = Math.Min(k1.Length, k2.Length);
            for (var i = 0; i < len; i++)
            {
                if (k1[i] != k2[i])
                {
                    return i;
                }
            }

            return len;
        }

        public void TailSort(List<Node> ns)
        {
            if (ns.Count > 1)
            {
                ns.Sort();
                for (var i = ns.Count - 1; i >= 0; i--)
                {
                    if (ns[i].Type > NtStatic && ns[i].Tail == '/')
                    {
                        (ns[i], ns[^1]) = (ns[^1], ns[i]);
                        return;
                    }
                }
            }
        }

        private List<Node> Append(List<Node>? src, Node child)
        {
            if (src == null)
            {
                return new List<Node> { child };
            }

            var result = new List<Node>();
            result.AddRange(src);
            result.Add(child);
            return result;
        }

        public Segment PatNextSegment(string pattern)
        {
            var ps = pattern.IndexOf('{');
            var ws = pattern.IndexOf('*');
            if (ps < 0 && ws < 0)
            {
                return new Segment(
                    NtStatic, "", ZeroChar, 0,
                    pattern.Length
                ); // we return the entire thing
            }

            // Sanity check
            // require(!(ps >= 0 && ws >= 0 && ws < ps)) { "chi: wildcard '*' must be the last pattern in a route, otherwise use a '{param}'" }
            if (ps >= 0 && ws >= 0 && ws < ps)
            {
                throw new Exception("chi: wildcard '*' must be the last pattern in a route, otherwise use a '{param}'");
            }

            var tail = '/'; // Default endpoint tail to / byte
            if (ps >= 0)
            {
                // Param/Regexp pattern is next
                var nt = NtParam;

                // Read to closing } taking into account opens and closes in curl count (cc)
                var cc = 0;
                var pe = ps;
                var range = pattern[ps..];
                for (var i = 0; i < range.Length; i++)
                {
                    var c = range[i];
                    if (c == '{')
                    {
                        cc++;
                    }
                    else if (c == '}')
                    {
                        cc--;
                        if (cc == 0)
                        {
                            pe = ps + i;
                            break;
                        }
                    }
                }

                // require(pe != ps) { "Router: route param closing delimiter '}' is missing" }
                if (pe == ps)
                {
                    throw new Exception("Router: route param closing delimiter '}' is missing");
                }

                var key = pattern[(ps + 1)..pe];
                pe++; // set end to next position
                if (pe < pattern.Length)
                {
                    tail = pattern[pe];
                }

                var rexpat = "";
                var idx = key.IndexOf(':');
                if (idx >= 0)
                {
                    nt = NtRegexp;
                    rexpat = key[(idx + 1)..];
                    //          key = key.substring(0, idx);
                }

                if (!string.IsNullOrEmpty(rexpat))
                {
                    if (rexpat[0] != '^')
                    {
                        rexpat = $"^{rexpat}";
                    }

                    if (rexpat[^1] != '$')
                    {
                        rexpat = $"{rexpat}$";
                    }
                }

                return new Segment(nt, rexpat, tail, ps, pe);
            }

            // Wildcard pattern as finale
            // EDIT: should we panic if there is stuff after the * ???
            // We allow naming a wildcard: *path
            // String key = ws == pattern.length() - 1 ? "*" : pattern.substring(ws + 1).toString();
            return new Segment(NtCatchAll, "", ZeroChar, ws, pattern.Length);
        }
    }
}