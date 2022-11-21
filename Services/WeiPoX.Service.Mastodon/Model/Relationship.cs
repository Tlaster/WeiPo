namespace WeiPoX.Service.Mastodon.Model;

public record Relationship
{
    /// <summary>
    ///     Target account id
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    /// <summary>
    ///     Whether the user is currently following the account
    /// </summary>
    [JsonPropertyName("following")]
    public bool? Following { get; init; }

    /// <summary>
    ///     Whether the user is currently being followed by the account
    /// </summary>
    [JsonPropertyName("followed_by")]
    public bool? FollowedBy { get; init; }

    /// <summary>
    ///     Whether the user is currently blocking the account
    /// </summary>
    [JsonPropertyName("blocking")]
    public bool? Blocking { get; init; }

    /// <summary>
    ///     Whether the user is currently muting the account
    /// </summary>
    [JsonPropertyName("muting")]
    public bool? Muting { get; init; }

    /// <summary>
    ///     Whether the user has requested to follow the account
    /// </summary>
    [JsonPropertyName("requested")]
    public bool? Requested { get; init; }

    /// <summary>
    ///     Whether the user is currently blocking the accounts's domain
    /// </summary>
    [JsonPropertyName("domain_blocking")]
    public bool? DomainBlocking { get; init; }
}