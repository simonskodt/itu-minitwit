using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;

namespace MiniTwit.Service.IServices;

public interface IFollowerService
{
    Response Create(string userId, string targetUsername);
    Task<Response> CreateAsync(string userId, string targetUsername);
    Response Delete(string userId, string targetUsername);
    Task<Response> DeleteAsync(string userId, string targetUsername);
    Response<IEnumerable<FollowerDTO>> GetAllFollowersByUsername(string username, CancellationToken ct = default);
    Task<Response<IEnumerable<FollowerDTO>>> GetAllFollowersByUsernameAsync(string username, CancellationToken ct = default);
}
