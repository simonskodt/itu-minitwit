using MiniTwit.Core.DTOs;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MiniTwit.Service.IServices;
using static MiniTwit.Core.Responses.HTTPResponse;

namespace MiniTwit.Service.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessageService(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public Response Create(string userId, string text)
    {
        var dbResult = _messageRepository.Create(userId, text);

        if (dbResult.ErrorType != null)
        {
            return new Response(NotFound, dbResult.ErrorType);
        }

        return new Response(NoContent);
    }

    public async Task<Response> CreateAsync(string userId, string text)
    {
        var dbResult = await _messageRepository.CreateAsync(userId, text);

        if (dbResult.ErrorType != null)
        {
            return new Response(NotFound, dbResult.ErrorType);
        }

        return new Response(NoContent);
    }

    public Response CreateByUsername(string username, string text)
    {
        var userDBResult = _userRepository.GetByUsername(username);

        if (userDBResult.ErrorType != null)
        {
            return new Response(NotFound, userDBResult.ErrorType);
        }

        var dbResult = _messageRepository.Create(userDBResult.Model!.Id, text);

        if (userDBResult.ErrorType != null)
        {
            return new Response(NotFound, dbResult.ErrorType);
        }

        return new Response(NoContent);
    }

    public async Task<Response> CreateByUsernameAsync(string username, string text)
    {
        var userDBResult = await _userRepository.GetByUsernameAsync(username);

        if (userDBResult.ErrorType != null)
        {
            return new Response(NotFound, userDBResult.ErrorType);
        }

        var dbResult = await _messageRepository.CreateAsync(userDBResult.Model!.Id, text);

        if (userDBResult.ErrorType != null)
        {
            return new Response(NotFound, dbResult.ErrorType);
        }

        return new Response(NoContent);
    }

    public Response<IEnumerable<MessageDTO>> GetAllByUserId(string userId, CancellationToken ct = default)
    {
        var dbResult = _messageRepository.GetAllByUserId(userId, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public async Task<Response<IEnumerable<MessageDTO>>> GetAllByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var dbResult = await _messageRepository.GetAllByUserIdAsync(userId, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllByUsername(string username, CancellationToken ct = default)
    {
        var dbResult = _messageRepository.GetAllByUsername(username, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public async Task<Response<IEnumerable<MessageDTO>>> GetAllByUsernameAsync(string username, CancellationToken ct = default)
    {
        var dbResult = await _messageRepository.GetAllByUsernameAsync(username, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllFollowedByUserId(string userId, CancellationToken ct = default)
    {
        var dbResult = _messageRepository.GetAllFollowedByUserId(userId, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public async Task<Response<IEnumerable<MessageDTO>>> GetAllFollowedByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var dbResult = await _messageRepository.GetAllFollowedByUserIdAsync(userId, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllNonFlagged(CancellationToken ct = default)
    {
        var dbResult = _messageRepository.GetAllNonFlagged(ct);

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public async Task<Response<IEnumerable<MessageDTO>>> GetAllNonFlaggedAsync(CancellationToken ct = default)
    {
        var dbResult = await _messageRepository.GetAllNonFlaggedAsync(ct);

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllNonFlaggedByUsername(string username, CancellationToken ct = default)
    {
        var dbResult = _messageRepository.GetAllNonFlaggedByUsername(username, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public async Task<Response<IEnumerable<MessageDTO>>> GetAllNonFlaggedByUsernameAsync(string username, CancellationToken ct = default)
    {
        var dbResult = await _messageRepository.GetAllNonFlaggedByUsernameAsync(username, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }
}
