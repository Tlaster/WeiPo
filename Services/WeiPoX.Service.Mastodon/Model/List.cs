namespace WeiPoX.Service.Mastodon.Model;

public record List
{
    [JsonPropertyName("id")] public string? Id { get; init; }

    [JsonPropertyName("title")] public string? Title { get; init; }
}