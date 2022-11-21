namespace WeiPoX.Service.Mastodon.Model;

public record Application
{
    /// <summary>
    ///     Name of the app
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    ///     Homepage URL of the app
    /// </summary>
    [JsonPropertyName("website")]
    public string? Website { get; init; }
}