using WeiPoX.Service.Mastodon.Api;

namespace WeiPoX.Service.Mastodon;

public interface IMastodonOAuthService : IAppsApi
{
    public static string OAuthUrl(string host, string clientId, string redirectUri, string responseType,
        bool forceLogin, string scope)
    {
        return
            $"https://{host}/oauth/authorize?client_id={clientId}&redirect_uri={redirectUri}&response_type={responseType}&scope={scope}&force_login={forceLogin}";
    }
}