using MiniTwit.Core.Entities;

namespace MiniTwit.Core.IRepositories;

public interface ILatestRepository
{
    Response<Latest> GetLatest();
}