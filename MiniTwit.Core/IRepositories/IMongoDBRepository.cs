using MiniTwit.Core.Entities;
using MongoDB.Bson;

namespace MiniTwit.Core.IRepositories;

public interface IMongoDBRepository
{
    void RegisterUser(string username, string email, string pw);
    public User? GetUserByUserName(string userName);
    ICollection<Message> DisplayAllTweets();
    Message? DisplayTweetByUserName(string userName, ObjectId userId);
    void FollowUser(string userName);
    void UnfollowUser(string userName);
    void AddMessage();
    public User? Login(string email, string pw);
    void Register();
    void Logout();
}