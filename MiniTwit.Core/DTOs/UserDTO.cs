namespace MiniTwit.Core.DTOs;

public record UserDTO
{
    public string? Id { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
}

public record UserCreateDTO
{
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
}
