using MongoDB.Driver;
using MongoDB.Bson;

public class Follower
{
    public ObjectId Who_id { get; set; }
    
    public ObjectId Whom_id { get; set; }
}