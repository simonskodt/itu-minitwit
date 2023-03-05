using MiniTwit.Core.Entities;
using MiniTwit.Core.Responses;

namespace MiniTwit.Core.IRepositories;

public interface IUserRepository
{
    DBResult<User> Create(string username, string email, string password);
    Task<DBResult<User>> CreateAsync(string username, string email, string password);
    DBResult<User> GetByUserId(string userId, CancellationToken ct = default);
    Task<DBResult<User>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    DBResult<User> GetByUsername(string username, CancellationToken ct = default);
    Task<DBResult<User>> GetByUsernameAsync(string username, CancellationToken ct = default);
}