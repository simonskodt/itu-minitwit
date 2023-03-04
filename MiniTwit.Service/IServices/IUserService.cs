using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;

namespace MiniTwit.Service.IServices;

public interface IUserService
{
    Response<UserDTO> GetByUserId(string userId, CancellationToken ct = default);
    Response<UserDTO> GetByUsername(string username, CancellationToken ct = default);
    Response Create(UserCreateDTO userCreateDTO);
}
