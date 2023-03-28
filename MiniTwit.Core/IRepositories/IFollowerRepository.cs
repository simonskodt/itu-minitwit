using MiniTwit.Core.Entities;
using MiniTwit.Core.Responses;

namespace MiniTwit.Core.IRepositories;

public interface IFollowerRepository
{
    DBResult<Follower> Create(string userId, string username);
    Task<DBResult<Follower>> CreateAsync(string userId, string username);
    DBResult Delete(string userId, string username);
    Task<DBResult> DeleteAsync(string userId, string username);
    DBResult<IEnumerable<Follower>> GetAllFollowersByUsername(string username, CancellationToken ct = default);
    Task<DBResult<IEnumerable<Follower>>> GetAllFollowersByUsernameAsync(string username, CancellationToken ct = default);
    Task<DBResult<bool?>> GetIsFollowed(string userId, string username);
}