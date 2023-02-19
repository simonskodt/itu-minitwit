using MiniTwit.Core.Entities;

namespace MiniTwit.Core.IRepositories;

public interface IUserRepository
{
    Response<User> GetByUserId(string userId);
    Response<User> GetByUsername(string username);
    Response<User> Create(string username, string email, string password);
}