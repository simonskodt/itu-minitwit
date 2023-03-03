namespace MiniTwit.Core.DTOs;

public record LoginDTO
{
    public string? Username { get; init; }
    public string? Password { get; init; }
}
