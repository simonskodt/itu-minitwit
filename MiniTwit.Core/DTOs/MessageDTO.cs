using System.Text.Json.Serialization;

namespace MiniTwit.Core.DTOs;

public record MessageDTO
{
    public string? Id { get; init; }
    public string? AuthorId { get; init; }
    public string? Text { get; init; }
    public DateTime PubDate { get; init; }
    public int Flagged { get; init; }
}

public record MessageDetailsDTO
{
    public string? Content { get; init; }
    public string? Username { get; init; }
    [JsonPropertyName("pub_date")]
    public DateTime PubDate { get; init; }
}

public record MessageCreateDTO
{
    public string? Content { get; init; }
}
