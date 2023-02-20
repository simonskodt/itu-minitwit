using MongoDB.Bson;

namespace MiniTwit.Core.Entities;

public class Follower
{
    public ObjectId Who_id { get; set; }
    
    public ObjectId Whom_id { get; set; }
}

public class FollowDTO
{
    public string? Who { get; set; }
    public string? Whom { get; set; }

}