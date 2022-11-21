namespace WeiPoX.Service.Mastodon.Model;

public record Notification
{
    /// <summary>
    ///     The notification ID
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    /// <summary>
    ///     One of: "mention", "reblog", "favourite", "follow"
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    ///     The time the notification was created
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    ///     The Account sending the notification to the user
    /// </summary>
    [JsonPropertyName("account")]
    public Account? Account { get; init; }

    /// <summary>
    ///     The Status associated with the notification, if applicible
    /// </summary>
    [JsonPropertyName("status")]
    public Status? Status { get; init; }
}