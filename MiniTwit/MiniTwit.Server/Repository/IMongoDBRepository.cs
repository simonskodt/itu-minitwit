namespace MiniTwit.Server.Repository;

public interface IMongoDBRepository
{
    public void InsertUser();
    public User? GetUserByUserName(string userName);
}