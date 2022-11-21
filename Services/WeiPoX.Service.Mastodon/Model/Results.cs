namespace WeiPoX.Service.Mastodon.Model;

public record Results
{
    /// <summary>
    ///     An array of matched Accounts
    /// </summary>
    [JsonPropertyName("accounts")]
    public IEnumerable<Account>? Accounts { get; init; }

    /// <summary>
    ///     An array of matchhed Statuses
    /// </summary>
    [JsonPropertyName("statuses")]
    public IEnumerable<Status>? Statuses { get; init; }

    /// <summary>
    ///     An array of matched hashtags, as strings
    /// </summary>
    [JsonPropertyName("hashtags")]
    public IEnumerable<string>? Hashtags { get; init; }
}