namespace WeiPoX.Service.Mastodon.Model;

public record Status
{
    /// <summary>
    ///     The ID of the status
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    /// <summary>
    ///     A Fediverse-unique resource ID
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    /// <summary>
    ///     URL to the status page (can be remote)
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    ///     The Account which posted the status
    /// </summary>
    [JsonPropertyName("account")]
    public Account? Account { get; init; }

    /// <summary>
    ///     null or the ID of the status it replies to
    /// </summary>
    [JsonPropertyName("in_reply_to_id")]
    public long? InReplyToId { get; init; }

    /// <summary>
    ///     null or the ID of the account it replies to
    /// </summary>
    [JsonPropertyName("in_reply_to_account_id")]
    public long? InReplyToAccountId { get; init; }

    /// <summary>
    ///     null or the reblogged Status
    /// </summary>
    [JsonPropertyName("reblog")]
    public Status? Reblog { get; init; }

    /// <summary>
    ///     Body of the status; this will contain HTML (remote HTML already sanitized)
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    /// <summary>
    ///     The time the status was created
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    ///     The number of reblogs for the status
    /// </summary>
    [JsonPropertyName("reblogs_count")]
    public long? ReblogCount { get; init; }

    /// <summary>
    ///     The number of favourites for the status
    /// </summary>
    [JsonPropertyName("favourites_count")]
    public long? FavouritesCount { get; init; }

    /// <summary>
    ///     Whether the authenticated user has reblogged the status
    /// </summary>
    [JsonPropertyName("reblogged")]
    public bool? Reblogged { get; init; }

    /// <summary>
    ///     Whether the authenticated user has favourited the status
    /// </summary>
    [JsonPropertyName("favourited")]
    public bool? Favourited { get; init; }

    /// <summary>
    ///     Whether the authenticated user has muted the conversation this status from
    /// </summary>
    [JsonPropertyName("muted")]
    public bool? Muted { get; init; }

    /// <summary>
    ///     Whether media attachments should be hidden by default
    /// </summary>
    [JsonPropertyName("sensitive")]
    public bool? Sensitive { get; init; }

    /// <summary>
    ///     If not empty, warning text that should be displayed before the actual content
    /// </summary>
    [JsonPropertyName("spoiler_text")]
    public string? SpoilerText { get; init; }

    /// <summary>
    ///     One of: public, unlisted, private, direct
    /// </summary>
    [JsonPropertyName("visibility")]
    public Visibility? Visibility { get; init; }

    /// <summary>
    ///     An array of Attachments
    /// </summary>
    [JsonPropertyName("media_attachments")]
    public IEnumerable<Attachment>? MediaAttachments { get; init; }

    /// <summary>
    ///     An array of Mentions
    /// </summary>
    [JsonPropertyName("mentions")]
    public IEnumerable<Mention>? Mentions { get; init; }

    /// <summary>
    ///     An array of Tags
    /// </summary>
    [JsonPropertyName("tags")]
    public IEnumerable<Tag>? Tags { get; init; }

    /// <summary>
    ///     Application from which the status was posted
    /// </summary>
    [JsonPropertyName("application")]
    public Application? Application { get; init; }

    /// <summary>
    ///     The detected language for the status, if detected
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; init; }
}