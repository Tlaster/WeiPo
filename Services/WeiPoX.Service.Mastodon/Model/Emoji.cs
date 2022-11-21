namespace WeiPoX.Service.Mastodon.Model;

public record Emoji
{
    [JsonPropertyName("shortcode")] public string? Shortcode { get; init; }

    [JsonPropertyName("static_url")] public string? StaticUrl { get; init; }

    [JsonPropertyName("url")] public string? Url { get; init; }
}