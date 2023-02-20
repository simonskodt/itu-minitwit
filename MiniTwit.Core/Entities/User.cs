using MongoDB.Bson;

namespace MiniTwit.Core.Entities;

public class User
{
    public ObjectId _id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public byte[]? PwHash { get; set; }
    public bool isloggedIn {get; set;}
}