namespace WeiPoX.Service.Mastodon.Model;

public record Auth
{
    [JsonPropertyName("access_token")] public string? AccessToken { get; init; }

    [JsonPropertyName("token_type")] public string? TokenType { get; init; }

    [JsonPropertyName("scope")] public string? Scope { get; init; }

    [JsonPropertyName("created_at")] public string? CreatedAt { get; init; }
}