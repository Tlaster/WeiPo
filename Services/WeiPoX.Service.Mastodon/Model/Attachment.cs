namespace WeiPoX.Service.Mastodon.Model;

public record Attachment
{
    /// <summary>
    ///     ID of the attachment
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    /// <summary>
    ///     One of: "image", "video", "gifv", "unknown"
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    ///     URL of the locally hosted version of the image
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    ///     For remote images, the remote URL of the original image
    /// </summary>
    [JsonPropertyName("remote_url")]
    public string? RemoteUrl { get; init; }

    /// <summary>
    ///     URL of the preview image
    /// </summary>
    [JsonPropertyName("preview_url")]
    public string? PreviewUrl { get; init; }

    /// <summary>
    ///     Shorter URL for the image, for insertion into text (only present on local images)
    /// </summary>
    [JsonPropertyName("text_url")]
    public string? TextUrl { get; init; }
}