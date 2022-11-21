namespace WeiPoX.Service.Mastodon.Model;

public record Mention
{
    /// <summary>
    ///     Account ID
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    /// <summary>
    ///     URL of user's profile (can be remote)
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    ///     The username of the account
    /// </summary>
    [JsonPropertyName("username")]
    public string? UserName { get; init; }

    /// <summary>
    ///     Equals username for local users, includes @domain for remote ones
    /// </summary>
    [JsonPropertyName("acct")]
    public string? AccountName { get; init; }
}