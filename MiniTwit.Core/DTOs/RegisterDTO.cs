using System.ComponentModel.DataAnnotations;

public record RegisterDTO
{
    [Required]
    public string? Username { get; init; }
    [Required]
    [EmailAddress]
    public string? Email { get; init; }
    [Required]
    public string? Password { get; init; }
}
