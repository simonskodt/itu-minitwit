using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;

namespace MiniTwit.Service.IServices;

public interface ILatestService
{
    Response<LatestDTO> Get(CancellationToken ct = default);
    Task<Response<LatestDTO>> GetAsync(CancellationToken ct = default);
    Response Update(int latestVal);
    Task<Response> UpdateAsync(int latestVal);
}
