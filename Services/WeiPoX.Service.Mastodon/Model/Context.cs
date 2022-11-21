namespace WeiPoX.Service.Mastodon.Model;

public record Context
{
    /// <summary>
    ///     The ancestors of the status in the conversation
    /// </summary>
    [JsonPropertyName("ancestors")]
    public IEnumerable<Status>? Ancestors { get; init; }

    /// <summary>
    ///     The descendants of the status in the conversation
    /// </summary>
    [JsonPropertyName("descendants")]
    public IEnumerable<Status>? Descendants { get; init; }
}