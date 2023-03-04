using MiniTwit.Core.Entities;
using MiniTwit.Core.Responses;

namespace MiniTwit.Core.IRepositories;

public interface IMessageRepository
{
    DBResult<IEnumerable<Message>> GetAllByUserId(string userId, CancellationToken ct = default);
    DBResult<IEnumerable<Message>> GetAllByUsername(string username, CancellationToken ct = default);
    DBResult<IEnumerable<Message>> GetAllNonFlaggedByUsername(string username, CancellationToken ct = default);
    DBResult<IEnumerable<Message>> GetAllNonFlagged(CancellationToken ct = default);
    DBResult<IEnumerable<Message>> GetAllFollowedByUserId(string userId, CancellationToken ct = default);
    DBResult<Message> Create(string userId, string text);
}