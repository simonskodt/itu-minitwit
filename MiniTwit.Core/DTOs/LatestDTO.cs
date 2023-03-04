using System.Text.Json.Serialization;

namespace MiniTwit.Core.DTOs;

public record LatestDTO
{
    [JsonPropertyName("latest")]
    public int LatestVal { get; init; }
}
