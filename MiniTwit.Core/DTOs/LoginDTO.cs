using System.ComponentModel.DataAnnotations;

public record LoginDTO
{
    [Required]
    public string? Username { get; init; }
    [Required]
    public string? Password { get; init; }
}
