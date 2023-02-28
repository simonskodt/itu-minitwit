using System.Text.Json.Serialization;

namespace MiniTwit.Core.DTOs;

public record SimFollowDTO
{
    [JsonPropertyName("follow")]
    public string? Follow { get; set; } = null;

    [JsonPropertyName("unfollow")]
    public string? Unfollow { get; set; } = null;
}

public record SimFollowDetailsDTO
{
    public IList<string> Follows { get; set; } = new List<string>();
}
