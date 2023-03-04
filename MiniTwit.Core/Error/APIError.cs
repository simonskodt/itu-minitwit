using System.Text.Json.Serialization;

namespace MiniTwit.Core.Error;

public class APIError
{
    public int Status { get; init; }
    [JsonPropertyName("error_msg")]
    public string ErrorMsg { get; set; } = null!;
}
