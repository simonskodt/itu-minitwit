using MiniTwit.Core.Entities;
using MongoDB.Bson;

namespace MiniTwit.Core.IRepositories;

public interface IMongoDBRepository
{
    public ICollection<Message> DisplayTimeline();
    public ICollection<Message> DisplayPublicTimeline();
    public ICollection<Message> DisplayUserTimeline();
    public Message? DisplayTweetByUserName(string userName, ObjectId userId);
    public void FollowUser(string userName);
    public void UnfollowUser(string userName);
    public void AddMessage(string text);
    public User? Login(string userName, string pw);
    public void RegisterUser(string username, string email, string pw);
    public void Logout();
    public User? GetUserByUserName(string userName);
}