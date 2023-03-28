using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;

namespace MiniTwit.Service.IServices;

public interface IMessageService
{
    Response Create(string userId, string text);
    Task<Response> CreateAsync(string userId, string text);
    Response CreateByUsername(string username, string text);
    Task<Response> CreateByUsernameAsync(string username, string text);
    Response<IEnumerable<MessageDTO>> GetAllByUserId(string userId, CancellationToken ct = default);
    Task<Response<IEnumerable<MessageDTO>>> GetAllByUserIdAsync(string userId, CancellationToken ct = default);
    Response<IEnumerable<MessageDTO>> GetAllByUsername(string username, CancellationToken ct = default);
    Task<Response<IEnumerable<MessageWithAutherNameDTO>>> GetAllByUsernameAsync(string username, CancellationToken ct = default);
    Response<IEnumerable<MessageDTO>> GetAllFollowedByUserId(string userId, CancellationToken ct = default);
    Task<Response<IEnumerable<MessageDTO>>> GetAllFollowedByUserIdAsync(string userId, CancellationToken ct = default);
    Response<IEnumerable<MessageDTO>> GetAllNonFlagged(CancellationToken ct = default);
    Task<Response<IEnumerable<MessageDTO>>> GetAllNonFlaggedAsync(CancellationToken ct = default);
    Task<Response<IEnumerable<MessageWithAutherNameDTO>>> GetAllNonFlaggedPageNumberLimitAsync(int pageNumber, CancellationToken ct = default);
    Response<IEnumerable<MessageDTO>> GetAllNonFlaggedByUsername(string username, CancellationToken ct = default);
    Task<Response<IEnumerable<MessageDTO>>> GetAllNonFlaggedByUsernameAsync(string username, CancellationToken ct = default);
}
