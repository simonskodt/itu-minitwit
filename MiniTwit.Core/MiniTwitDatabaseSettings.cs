namespace MiniTwit.Core;

public class MiniTwitDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string UsersCollectionName { get; set; } = null!;
    public string TweetsCollectionName { get; set; } = null!;
    public string FollowersCollectionName { get; set; } = null!;
}