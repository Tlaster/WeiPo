namespace WeiPoX.Service.Mastodon.Model;

public record Account
{
    /// <summary>
    ///     The ID of the account
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

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

    /// <summary>
    ///     The account's display name
    /// </summary>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; init; }

    /// <summary>
    ///     Boolean for when the account cannot be followed without waiting for approval first
    /// </summary>
    [JsonPropertyName("locked")]
    public bool? Locked { get; init; }

    /// <summary>
    ///     The time the account was created
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    ///     The number of followers for the account
    /// </summary>
    [JsonPropertyName("followers_count")]
    public long? FollowersCount { get; init; }

    /// <summary>
    ///     The number of accounts the given account is following
    /// </summary>
    [JsonPropertyName("following_count")]
    public long? FollowingCount { get; init; }

    /// <summary>
    ///     The number of statuses the account has made
    /// </summary>
    [JsonPropertyName("statuses_count")]
    public long? StatusesCount { get; init; }

    /// <summary>
    ///     Biography of user
    /// </summary>
    [JsonPropertyName("note")]
    public string? Note { get; init; }

    /// <summary>
    ///     URL of the user's profile page (can be remote)
    /// </summary>
    [JsonPropertyName("url")]
    public string? ProfileUrl { get; init; }

    /// <summary>
    ///     URL to the avatar image
    /// </summary>
    [JsonPropertyName("avatar")]
    public string? AvatarUrl { get; init; }

    /// <summary>
    ///     URL to the avatar static image (gif)
    /// </summary>
    [JsonPropertyName("avatar_static")]
    public string? StaticAvatarUrl { get; init; }

    /// <summary>
    ///     URL to the header image
    /// </summary>
    [JsonPropertyName("header")]
    public string? HeaderUrl { get; init; }

    /// <summary>
    ///     URL to the header image
    /// </summary>
    [JsonPropertyName("header_static")]
    public string? StaticHeaderUrl { get; init; }
}