using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;

namespace MiniTwit.Service.IServices;

public interface IMessageService
{
    Response<IEnumerable<MessageDTO>> GetAllByUserId(string userId, CancellationToken ct = default);
    Response<IEnumerable<MessageDTO>> GetAllByUsername(string username, CancellationToken ct = default);
    Response<IEnumerable<MessageDTO>> GetAllNonFlaggedByUsername(string username, CancellationToken ct = default);
    Response<IEnumerable<MessageDTO>> GetAllNonFlagged(CancellationToken ct = default);
    Response<IEnumerable<MessageDTO>> GetAllFollowedByUserId(string userId, CancellationToken ct = default);
    Response Create(string userId, string text);
    Response CreateByUsername(string username, string text);
}
