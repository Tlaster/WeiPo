namespace WeiPoX.Service.Mastodon.Model;

public record Card
{
    /// <summary>
    ///     The url associated with the card
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    ///     The title of the card
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    ///     The card description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    ///     The image associated with the card, if any
    /// </summary>
    [JsonPropertyName("image")]
    public string? Image { get; init; }

    /// <summary>
    ///     "link", "photo", "video", or "rich"
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    ///     OEmbed data
    /// </summary>
    [JsonPropertyName("author_name")]
    public string? AuthorName { get; init; }

    /// <summary>
    ///     OEmbed data
    /// </summary>
    [JsonPropertyName("author_url")]
    public string? AuthorUrl { get; init; }

    /// <summary>
    ///     OEmbed data
    /// </summary>
    [JsonPropertyName("provider_name")]
    public string? ProviderName { get; init; }

    /// <summary>
    ///     OEmbed data
    /// </summary>
    [JsonPropertyName("provider_url")]
    public string? ProviderUrl { get; init; }

    /// <summary>
    ///     OEmbed data
    /// </summary>
    [JsonPropertyName("html")]
    public string? Html { get; init; }

    /// <summary>
    ///     OEmbed data
    /// </summary>
    [JsonPropertyName("width")]
    public long? Width { get; init; }

    /// <summary>
    ///     OEmbed data
    /// </summary>
    [JsonPropertyName("height")]
    public long? Height { get; init; }
}