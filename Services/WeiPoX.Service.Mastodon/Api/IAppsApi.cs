using Refit;
using WeiPoX.Service.Mastodon.Model;

namespace WeiPoX.Service.Mastodon.Api;

public interface IAppsApi
{
    [Post("/api/v1/apps")]
    Task<CreateAppResponse> CreateApp([Body(BodySerializationMethod.UrlEncoded)] CreateAppRequest request);

    [Post("/oauth/token")]
    Task<OAuthTokenResponse> GetToken([Body(BodySerializationMethod.UrlEncoded)] OAuthTokenRequest request);

    [Post("/oauth/revoke")]
    Task RevokeToken([Body(BodySerializationMethod.UrlEncoded)] RevokeTokenRequest request);
}