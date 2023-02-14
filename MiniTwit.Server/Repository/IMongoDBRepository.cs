using MiniTwit.Server.Entities;

namespace MiniTwit.Server.Repository;

public interface IMongoDBRepository
{
    void RegisterUser(string username, string email, string pw);
    public User? GetUserByUserName(string userName);
    ICollection<Message> DisplayAllTweets();
    Message? DisplayTweetByUserName(string userName);
    void FollowUser(string userName);
    void UnfollowUser(string userName);
    void AddMessage();
    public User? Login(string email, string pw);
    void Register();
    void Logout();
}