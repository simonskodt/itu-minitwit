using System.Text.Json.Serialization;

namespace MiniTwit.Core.Entities;

public record SimFollowDTO
{
    [JsonPropertyName("follow")]
    public string? Follow { get; set; } = null;

    [JsonPropertyName("unfollow")]
    public string? Unfollow { get; set; } = null;
}