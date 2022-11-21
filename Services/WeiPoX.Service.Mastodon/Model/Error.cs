namespace WeiPoX.Service.Mastodon.Model;

public record Error
{
    /// <summary>
    ///     A textual description of the error
    /// </summary>
    [JsonPropertyName("error")]
    public string? Description { get; init; }
}