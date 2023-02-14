using MongoDB.Bson;

namespace MiniTwit.Server.Entities;

public class Follower
{
    public ObjectId Who_id { get; set; }
    
    public ObjectId Whom_id { get; set; }
}