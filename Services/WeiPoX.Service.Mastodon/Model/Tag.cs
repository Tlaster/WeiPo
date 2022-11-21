namespace WeiPoX.Service.Mastodon.Model;

public record Tag
{
    /// <summary>
    ///     The hashtag, not including the preceding #
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    ///     The URL of the hashtag
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}