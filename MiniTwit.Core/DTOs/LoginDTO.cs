using System.ComponentModel.DataAnnotations;

namespace MiniTwit.Core.Entities;

public record LoginDTO
{
    [Required]
    public string? Username { get; init; }
    [Required]
    public string? Password { get; init; }
}