using MiniTwit.Core.Entities;

namespace MiniTwit.Core.IRepositories;

public interface IFollowerRepository
{
    Response<Follower> Create(string userId, string targetUsername);
    Response Delete(string userId, string targetUsername);
}