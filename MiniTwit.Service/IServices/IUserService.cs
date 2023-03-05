using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;

namespace MiniTwit.Service.IServices;

public interface IUserService
{
    Response Create(UserCreateDTO userCreateDTO);
    Task<Response> CreateAsync(UserCreateDTO userCreateDTO);
    Response<UserDTO> GetByUserId(string userId, CancellationToken ct = default);
    Task<Response<UserDTO>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Response<UserDTO> GetByUsername(string username, CancellationToken ct = default);
    Task<Response<UserDTO>> GetByUsernameAsync(string username, CancellationToken ct = default);
}
