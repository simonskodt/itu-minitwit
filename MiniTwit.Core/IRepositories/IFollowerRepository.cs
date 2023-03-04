using MiniTwit.Core.Entities;
using MiniTwit.Core.Responses;

namespace MiniTwit.Core.IRepositories;

public interface IFollowerRepository
{
    DBResult<Follower> Create(string userId, string username);
    DBResult Delete(string userId, string username);
    DBResult<IEnumerable<Follower>> GetAllFollowersByUsername(string username, CancellationToken ct = default);
}