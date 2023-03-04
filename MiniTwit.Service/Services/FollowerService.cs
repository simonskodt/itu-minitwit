using MiniTwit.Core.DTOs;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MiniTwit.Service.IServices;
using static MiniTwit.Core.Responses.HTTPResponse;

namespace MiniTwit.Service.Services;

public class FollowerService : IFollowerService
{
    private readonly IFollowerRepository _repository;

    public FollowerService(IFollowerRepository repository)
    {
        _repository = repository;
    }

    public Response Create(string userId, string targetUsername)
    {
        var dbResult = _repository.Create(userId, targetUsername);
        
        if (dbResult.Model == null)
        {
            return new Response(NotFound, dbResult.ErrorType);
        }

        return new Response(NoContent);
    }

    public Response Delete(string userId, string targetUsername)
    {
        var dbResult = _repository.Delete(userId, targetUsername);

        if (dbResult.ErrorType != null)
        {
            return new Response(NotFound, dbResult.ErrorType);
        }

        return new Response(NoContent);
    }

    public Response<IEnumerable<FollowerDTO>> GetAllFollowersByUsername(string username, CancellationToken ct = default)
    {
        var dbResult = _repository.GetAllFollowersByUsername(username, ct);

        if (dbResult.Model == null)
        {
            return new Response<IEnumerable<FollowerDTO>>(NotFound, null, dbResult.ErrorType);
        }

        return new Response<IEnumerable<FollowerDTO>>(Ok, dbResult.ConvertModelTo<IEnumerable<FollowerDTO>>());
    }
}
