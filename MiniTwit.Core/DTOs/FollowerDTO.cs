namespace MiniTwit.Core.DTOs;

public record FollowerDTO
{
    public string? WhoId { get; init; }
    public string? WhomId { get; init; }
}

public record FollowerCreateDTO
{
    public string? Follow { get; init; }
    public string? Unfollow { get; init; }
}

public record FollowerDetailsDTO
{
    public ICollection<string> Follows { get; init; } = new List<string>();
}
