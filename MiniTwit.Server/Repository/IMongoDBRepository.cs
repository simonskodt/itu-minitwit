using MiniTwit.Server.Entities;

namespace MiniTwit.Server.Repository;

public interface IMongoDBRepository
{
    void InsertUser();
    User? GetUserByUserName(string userName);
    ICollection<Message> DisplayAllTweets();
    Message? DisplayTweetByUserName(string userName);
    void FollowUser(string userName);
    void UnfollowUser(string userName);
    void AddMessage();
    void Login();
    void Register();
    void Logout();
}