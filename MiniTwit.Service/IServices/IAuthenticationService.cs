using MiniTwit.Core.Responses;

namespace MiniTwit.Service.IServices;

public interface IAuthenticationService
{
    Response Authenticate(string username, string password);
    Task<Response> AuthenticateAsync(string username, string password);
}
