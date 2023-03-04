using MiniTwit.Core.Entities;
using MiniTwit.Core.Responses;

namespace MiniTwit.Core.IRepositories;

public interface IUserRepository
{
    DBResult<User> GetByUserId(string userId, CancellationToken ct = default);
    DBResult<User> GetByUsername(string username, CancellationToken ct = default);
    DBResult<User> Create(string username, string email, string password);
}