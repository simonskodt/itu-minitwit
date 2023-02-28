using System.Text.Json.Serialization;

namespace MiniTwit.Core.DTOs;

public record SimMessageDTO
{
    [JsonPropertyName("content")]
    public string? Content { get; init; }
    [JsonPropertyName("username")]
    public string? Username { get; init; }
    [JsonPropertyName("pub_date")]
    public DateTime PubDate { get; init; }
}
