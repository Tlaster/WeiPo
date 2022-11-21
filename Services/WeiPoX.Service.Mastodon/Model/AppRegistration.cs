namespace WeiPoX.Service.Mastodon.Model;

public record AppRegistration
{
    [JsonPropertyName("id")] public long? Id { get; init; }

    [JsonPropertyName("redirect_uri")] public string? RedirectUri { get; init; }

    [JsonPropertyName("client_id")] public string? ClientId { get; init; }

    [JsonPropertyName("client_secret")] public string? ClientSecret { get; init; }

    [JsonIgnore] public string? Instance { get; init; }

    [JsonIgnore] public Scope? Scope { get; init; }
}