using MiniTwit.Core.Entities;
using MiniTwit.Core.Responses;

namespace MiniTwit.Core.IRepositories;

public interface IMessageRepository
{
    DBResult<Message> Create(string userId, string text);
    Task<DBResult<Message>> CreateAsync(string userId, string text);
    DBResult<IEnumerable<Message>> GetAllNonFlagged(CancellationToken ct = default);
    Task<DBResult<IEnumerable<Message>>> GetAllNonFlaggedAsync(CancellationToken ct = default);
    Task<DBResult<IEnumerable<Message>>> GetAllNonFlaggedPageNumberLimitAsync(int pageNumber, CancellationToken ct = default);
    DBResult<IEnumerable<Message>> GetAllByUserId(string userId, CancellationToken ct = default);
    Task<DBResult<IEnumerable<Message>>> GetAllByUserIdAsync(string userId, CancellationToken ct = default);
    DBResult<IEnumerable<Message>> GetAllByUsername(string username, CancellationToken ct = default);
    Task<DBResult<IEnumerable<Message>>> GetAllByUsernameAsync(string username, CancellationToken ct = default);
    DBResult<IEnumerable<Message>> GetAllNonFlaggedByUsername(string username, CancellationToken ct = default);
    Task<DBResult<IEnumerable<Message>>> GetAllNonFlaggedByUsernameAsync(string username, CancellationToken ct = default);
    DBResult<IEnumerable<Message>> GetAllFollowedByUserId(string userId, CancellationToken ct = default);
    Task<DBResult<IEnumerable<Message>>> GetAllFollowedByUserIdAsync(string userId, CancellationToken ct = default);
    Task IndexDB();
}