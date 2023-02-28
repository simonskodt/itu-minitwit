using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniTwit.Core.Entities;

[BsonIgnoreExtraElements]
public class Follower
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string? WhoId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? WhomId { get; set; }
}
