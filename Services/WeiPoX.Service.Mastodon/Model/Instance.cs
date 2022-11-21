namespace WeiPoX.Service.Mastodon.Model;

public record Instance
{
    /// <summary>
    ///     URI of the current instance
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    /// <summary>
    ///     The instance's title
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    ///     A description for the instance
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    ///     An email address which can be used to contact the instance administrator
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }
}