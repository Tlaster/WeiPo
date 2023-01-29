namespace WeiPoX.Data.Common;

public record MicroBlogKey(string Id, string Host)
{
    public override string ToString()
    {
        return $"{Id}@{Host}";
    }

    public static MicroBlogKey Parse(string key)
    {
        var split = key.Split('@');
        if (split.Length != 2)
        {
            throw new ArgumentException("Invalid microblog key format.");
        }

        return new MicroBlogKey(split[0], split[1]);
    }
}