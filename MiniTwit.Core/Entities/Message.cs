using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniTwit.Core.Entities;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? MessageId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? AuthorId { get; set; }
    public string? Text { get; set; }
    public DateTime? PubDate { get; set; }
    public int Flagged { get; set; }
}
