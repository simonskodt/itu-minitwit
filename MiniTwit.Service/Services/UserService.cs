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

    public Response Create(UserCreateDTO userCreateDTO)
    {
        var dbResult = _repository.GetByUsername(userCreateDTO.Username!);

        if (dbResult.Model != null)
        {
            return new Response(BadRequest, USERNAME_TAKEN);
        }

        if (userCreateDTO.Username! == "")
        {
            return new Response(BadRequest, USERNAME_MISSING);
        }

        if (!userCreateDTO.Email!.Contains("@"))
        {
            return new Response(BadRequest, EMAIL_MISSING_OR_INVALID);
        }

        if (userCreateDTO.Password! == "")
        {
            return new Response(BadRequest, PASSWORD_MISSING);   
        }

        var passwordHash = _hasher.Hash(userCreateDTO.Password!);

        dbResult = _repository.Create(userCreateDTO.Username!, userCreateDTO.Email!, passwordHash);

        return new Response(NoContent);
    }

    public async Task<Response> CreateAsync(UserCreateDTO userCreateDTO)
    {
        var dbResult = await _repository.GetByUsernameAsync(userCreateDTO.Username!);

        if (dbResult.Model != null)
        {
            return new Response(BadRequest, USERNAME_TAKEN);
        }

        if (userCreateDTO.Username! == "")
        {
            return new Response(BadRequest, USERNAME_MISSING);
        }

        if (!userCreateDTO.Email!.Contains("@"))
        {
            return new Response(BadRequest, EMAIL_MISSING_OR_INVALID);
        }

        if (userCreateDTO.Password! == "")
        {
            return new Response(BadRequest, PASSWORD_MISSING);   
        }

        var passwordHash = await _hasher.HashAsync(userCreateDTO.Password!);

        dbResult = await _repository.CreateAsync(userCreateDTO.Username!, userCreateDTO.Email!, passwordHash);

        return new Response(NoContent);
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

    public async Task<Response<UserDTO>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var dbResult = await _repository.GetByUserIdAsync(userId, ct);

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
            return new Response<UserDTO>(BadRequest, null, dbResult.ErrorType);
        }

        return new Response<UserDTO>(Ok, dbResult.ConvertModelTo<UserDTO>());
    }

    public async Task<Response<UserDTO>> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        var dbResult = await _repository.GetByUsernameAsync(username, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<UserDTO>(BadRequest, null, dbResult.ErrorType);
        }

        return new Response<UserDTO>(Ok, dbResult.ConvertModelTo<UserDTO>());
    }

}
