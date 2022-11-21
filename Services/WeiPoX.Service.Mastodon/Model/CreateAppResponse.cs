using Refit;

namespace WeiPoX.Service.Mastodon.Model;

public record CreateAppResponse
{
    [JsonPropertyName("name")] public string? Name { get; init; }
    [JsonPropertyName("website")] public string? Website { get; init; }
    [JsonPropertyName("vapid_key")] public string? VapidKey { get; init; }
    [JsonPropertyName("client_id")] public string? ClientId { get; init; }
    [JsonPropertyName("client_secret")] public string? ClientSecret { get; init; }
    [JsonPropertyName("redirect_uri")] public string? RedirectUri { get; init; }
}

public record CreateAppRequest
(
    [AliasAs("client_name")] string? ClientName,
    [AliasAs("redirect_uris")] string? RedirectUris,
    [AliasAs("scopes")] string? Scopes,
    [AliasAs("website")] string? Website
);