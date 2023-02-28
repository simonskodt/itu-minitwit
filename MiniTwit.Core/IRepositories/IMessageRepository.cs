using MiniTwit.Core.Entities;

namespace MiniTwit.Core.IRepositories;

public interface IMessageRepository
{
    Response<IEnumerable<Message>> GetAllByUserId(string userId);
    Response<IEnumerable<Message>> GetAllByUsername(string username);
    Response<IEnumerable<Message>> GetAllNonFlaggedByUsername(string username);
    Response<IEnumerable<Message>> GetAllNonFlagged();
    Response<IEnumerable<Message>> GetAllFollowedByUser(string userId);
    Response<Message> Create(string userId, string text);
}