using System.ComponentModel.DataAnnotations;

namespace MiniTwit.Core.DTOs;

public record LoginDTO
{
    [Required]
    public string? Username { get; init; }
    [Required]
    public string? Password { get; init; }
}
