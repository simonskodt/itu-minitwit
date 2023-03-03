namespace MiniTwit.Core.DTOs;

public record FollowerDTO
{
    public string? WhoId { get; init; }
    public string? WhomId { get; init; }
}

public record SimFollowerDTO
{
    public string? Follow { get; set; } = null;
    public string? Unfollow { get; set; } = null;
}

public record SimFollowerDetailsDTO
{
    public ICollection<string> Follows { get; set; } = new List<string>();
}
