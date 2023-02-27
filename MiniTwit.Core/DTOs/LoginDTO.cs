namespace MiniTwit.Core.Entities;

public record LoginDTO
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}