using MiniTwit.Core.Error;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MiniTwit.Security;
using MiniTwit.Service.IServices;
using static MiniTwit.Core.Responses.HTTPResponse;
using static MiniTwit.Core.Error.ErrorType;

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

    public Response Authenticate(string username, string password)
    {
        var dbResult = _repository.GetByUsername(username);

        if (dbResult.ErrorType == ErrorType.INVALID_USERNAME)
        {
            return new Response(Unauthorized, dbResult.ErrorType);
        }

        var validPassword = _hasher.VerifyHash(password, dbResult.Model!.Password);

        if (!validPassword)
        {
            return new Response(Unauthorized, INVALID_PASSWORD);
        }

        return new Response(NoContent, null);
    }
}
