using MiniTwit.Core.DTOs;
using MiniTwit.Core.IRepositories;
using MiniTwit.Service.IServices;
using MiniTwit.Core.Responses;
using static MiniTwit.Core.Responses.HTTPResponse;

namespace MiniTwit.Service.Services;

public class LatestService : ILatestService
{
    private readonly ILatestRepository _repository;
    
    public LatestService(ILatestRepository repository)
    {
        _repository = repository;
    }

    public Response<LatestDTO> Get(CancellationToken ct = default)
    {
        var dbResult = _repository.Get(ct);

        return new Response<LatestDTO>(Ok, dbResult.ConvertModelTo<LatestDTO>());
    }

    public async Task<Response<LatestDTO>> GetAsync(CancellationToken ct = default)
    {
        var dbResult = await _repository.GetAsync(ct);

        return new Response<LatestDTO>(Ok, dbResult.ConvertModelTo<LatestDTO>());
    }

    public Response Update(int latestVal)
    {
        var dbResult = _repository.Update(latestVal);

        return new Response(NoContent);
    }

    public async Task<Response> UpdateAsync(int latestVal)
    {
        var dbResult = await _repository.UpdateAsync(latestVal);

        return new Response(NoContent);
    }
}
