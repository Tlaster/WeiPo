namespace WeiPoX.Core.Routing.Parser;

internal class RouteMatch
{
    public bool Matches { get; set; }
    public Route? Route { get; set; }
    public List<string> Vars { get; set; } = new();
    public List<string> Keys { get; set; } = new();
    public Dictionary<string, string> PathMap { get; set; } = new();

    public void Key()
    {
        var size = Math.Min(Keys.Count, Vars.Count);
        for (var i = 0; i < size; i++)
        {
            PathMap[Keys[i]] = Vars[i];
        }

        for (var i = 0; i < size; i++)
        {
            Vars.RemoveAt(0);
        }
    }

    public void Truncate(int size)
    {
        var sizeInt = size;
        while (sizeInt < Vars.Count)
        {
            Vars.RemoveAt(sizeInt++);
        }
    }

    public void Value(string value)
    {
        Vars.Add(value);
    }

    public void Pop()
    {
        if (Vars.Count > 0)
        {
            Vars.RemoveAt(Vars.Count - 1);
        }
    }

    public RouteMatch Found(Route route)
    {
        Route = route;
        Matches = true;
        return this;
    }
}