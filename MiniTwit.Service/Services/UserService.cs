using MiniTwit.Core.DTOs;
using MiniTwit.Core.IRepositories;
using MiniTwit.Security;
using MiniTwit.Service.IServices;
using MiniTwit.Core.Responses;
using static MiniTwit.Core.Responses.HTTPResponse;
using static MiniTwit.Core.Error.ErrorType;

namespace MiniTwit.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IHasher _hasher;
    
    public UserService(IUserRepository repository, IHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public Response<UserDTO> Create(UserCreateDTO userCreateDTO)
    {
        var dbResult = _repository.GetByUsername(userCreateDTO.Username!);

        // Username taken
        if (dbResult.ErrorType != null)
        {
            return new Response<UserDTO>(Conflict, null, USERNAME_TAKEN);
        }

        _hasher.Hash(userCreateDTO.Password!, out string passwordHash);

        dbResult = _repository.Create(userCreateDTO.Username!, userCreateDTO.Email!, passwordHash);

        return new Response<UserDTO>(Created, dbResult.ConvertModelTo<UserDTO>());
    }

    public Response<UserDTO> GetByUserId(string userId, CancellationToken ct = default)
    {
        var dbResult = _repository.GetByUserId(userId, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<UserDTO>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<UserDTO>(Ok, dbResult.ConvertModelTo<UserDTO>());
    }

    public Response<UserDTO> GetByUsername(string username, CancellationToken ct = default)
    {
        var dbResult = _repository.GetByUsername(username, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<UserDTO>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<UserDTO>(Ok, dbResult.ConvertModelTo<UserDTO>());
    }
}
