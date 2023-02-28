using System.Text.Json.Serialization;

namespace MiniTwit.Core.DTOs;

public record LastestDTO
{
    [JsonPropertyName("latest")]
    public int LatestVal { get; init; }
}
