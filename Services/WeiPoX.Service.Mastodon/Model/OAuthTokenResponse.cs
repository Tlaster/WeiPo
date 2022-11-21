using Refit;

namespace WeiPoX.Service.Mastodon.Model;

public record OAuthTokenResponse
{
    [JsonPropertyName("access_token")] public string? AccessToken { get; init; }
    [JsonPropertyName("token_type")] public string? TokenType { get; init; }
    [JsonPropertyName("scope")] public string? Scope { get; init; }
    [JsonPropertyName("created_at")] public long? CreatedAt { get; init; }
}

public record OAuthTokenRequest
(
    [AliasAs("grant_type")] string GrantType,
    [AliasAs("code")] string Code,
    [AliasAs("client_id")] string ClientId,
    [AliasAs("redirect_uri")] string RedirectUri,
    [AliasAs("scope")] string Scope
);

public record RevokeTokenRequest
(
    [AliasAs("client_id")] string ClientId,
    [AliasAs("client_secret")] string ClientSecret,
    [AliasAs("token")] string Token
);