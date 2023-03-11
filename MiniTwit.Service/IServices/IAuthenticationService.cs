using MiniTwit.Core.DTOs;
using MiniTwit.Core.Responses;

namespace MiniTwit.Service.IServices;

public interface IAuthenticationService
{
    Response<UserDTO> Authenticate(string username, string password);
    Task<Response<UserDTO>> AuthenticateAsync(string username, string password);
}
