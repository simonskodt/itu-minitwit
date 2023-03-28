using MiniTwit.Core.Error;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MiniTwit.Security;
using MiniTwit.Service.IServices;
using static MiniTwit.Core.Responses.HTTPResponse;
using static MiniTwit.Core.Error.ErrorType;
using MiniTwit.Core.DTOs;

namespace MiniTwit.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _repository;
    private readonly IHasher _hasher;

    public AuthenticationService(IUserRepository repository, IHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public Response<UserDTO> Authenticate(string username, string password)
    {
        var dbResult = _repository.GetByUsername(username);

        if (dbResult.ErrorType == ErrorType.INVALID_USERNAME)
        {
            return new Response<UserDTO>(Unauthorized, null, dbResult.ErrorType);
        }

        if (string.IsNullOrEmpty(password)) {
            return new Response<UserDTO>(Unauthorized, null, ErrorType.INVALID_PASSWORD);
        }

        var validPassword = _hasher.VerifyHash(password, dbResult.Model!.Password);

        if (!validPassword)
        {
            return new Response<UserDTO>(Unauthorized, null, INVALID_PASSWORD);
        }

        return new Response<UserDTO>(Ok, dbResult.ConvertModelTo<UserDTO>());
    }

    public async Task<Response<UserDTO>> AuthenticateAsync(string username, string password)
    {
        var dbResult = await _repository.GetByUsernameAsync(username);

        if (dbResult.ErrorType == ErrorType.INVALID_USERNAME)
        {
            return new Response<UserDTO>(Unauthorized, null, dbResult.ErrorType);
        }

        if (string.IsNullOrEmpty(password)) {
            return new Response<UserDTO>(Unauthorized, null, ErrorType.INVALID_PASSWORD);
        }

        var validPassword = await _hasher.VerifyHashAsync(password, dbResult.Model!.Password);

        if (!validPassword)
        {
            return new Response<UserDTO>(Unauthorized, null, INVALID_PASSWORD);
        }

        return new Response<UserDTO>(Ok, dbResult.ConvertModelTo<UserDTO>(), null);
    }
}
