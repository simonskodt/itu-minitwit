using MiniTwit.Core.Entities;

namespace MiniTwit.Infrastructure.Data.DataCreators;

public static class UserCreator
{
    public static User Create(string username, string email, string password)
    {
        return new User
        {
            Username = username,
            Email = email,
            Password = password,
        };
    }

    public static User Create(string id, string username, string email, string password)
    {
        return new User
        {
            Id = id,
            Username = username,
            Email = email,
            Password = password,
        };
    }
}
