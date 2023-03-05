using MiniTwit.Core.Entities;
using MiniTwit.Core.Responses;

namespace MiniTwit.Core.IRepositories;

public interface ILatestRepository
{
    DBResult<Latest> Get(CancellationToken ct = default);
    Task<DBResult<Latest>> GetAsync(CancellationToken ct = default);
    DBResult<Latest> Update(int latestVal);
    Task<DBResult<Latest>> UpdateAsync(int latestVal);
}
