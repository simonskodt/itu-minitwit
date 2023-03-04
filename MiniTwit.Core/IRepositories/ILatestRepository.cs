using MiniTwit.Core.Entities;
using MiniTwit.Core.Responses;

namespace MiniTwit.Core.IRepositories;

public interface ILatestRepository
{
    DBResult<Latest> Get(CancellationToken ct = default);
    DBResult<Latest> Update(int latestVal);
}
