using MiniTwit.Core.DTOs;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MiniTwit.Service.IServices;
using static MiniTwit.Core.Responses.HTTPResponse;

namespace MiniTwit.Service.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;

    public MessageService(IMessageRepository repository)
    {
        _repository = repository;
    }

    public Response<MessageDTO> Create(string userId, string text)
    {
        var dbResult = _repository.Create(userId, text);

        if (dbResult.ErrorType != null)
        {
            return new Response<MessageDTO>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<MessageDTO>(Created, dbResult.ConvertModelTo<MessageDTO>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllByUserId(string userId, CancellationToken ct = default)
    {
        var dbResult = _repository.GetAllByUserId(userId, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllByUsername(string username, CancellationToken ct = default)
    {
        var dbResult = _repository.GetAllByUsername(username, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllFollowedByUserId(string userId, CancellationToken ct = default)
    {
        var dbResult = _repository.GetAllFollowedByUserId(userId, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllNonFlagged(CancellationToken ct = default)
    {
        var dbResult = _repository.GetAllNonFlagged(ct);

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }

    public Response<IEnumerable<MessageDTO>> GetAllNonFlaggedByUsername(string username, CancellationToken ct = default)
    {
        var dbResult = _repository.GetAllNonFlaggedByUsername(username, ct);

        if (dbResult.ErrorType != null)
        {
            return new Response<IEnumerable<MessageDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<MessageDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<MessageDTO>>());
    }
}
