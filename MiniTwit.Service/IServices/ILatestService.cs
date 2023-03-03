using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;

namespace MiniTwit.Service.IServices;

public interface ILatestService
{
    Response<LatestDTO> Get(CancellationToken ct = default);
    Response<LatestDTO> Update(int latestVal);
}
