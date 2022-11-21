namespace WeiPoX.Service.Mastodon.Model;

public record Report
{
    /// <summary>
    ///     The ID of the report
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    /// <summary>
    ///     The action taken in response to the report
    /// </summary>
    [JsonPropertyName("action_taken")]
    public string? ActionTaken { get; init; }
}