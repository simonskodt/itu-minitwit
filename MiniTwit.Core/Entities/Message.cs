using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniTwit.Core.Entities;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    public string AuthorId { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime PubDate { get; set; }
    public int Flagged { get; set; }
}
