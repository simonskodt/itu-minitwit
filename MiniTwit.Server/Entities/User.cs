using MongoDB.Bson;

namespace MiniTwit.Server.Entities;

public class User
{
    public ObjectId _id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PwHash { get; set; }
}