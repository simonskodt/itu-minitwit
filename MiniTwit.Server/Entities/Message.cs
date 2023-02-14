using MongoDB.Bson;

namespace MiniTwit.Server.Entities;

public class Message
{
    public ObjectId MessageId { get; set; }
    public ObjectId AuthorId { get; set; }
    public string? Text { get; set; }
    public int? Flagged { get; set; }
}