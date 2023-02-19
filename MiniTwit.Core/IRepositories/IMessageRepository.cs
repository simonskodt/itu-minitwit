using MiniTwit.Core.Entities;

namespace MiniTwit.Core.IRepositories;

public interface IMessageRepository
{
    Response<IEnumerable<Message>> GetAllByUserId(string userId);
    Response<IEnumerable<Message>> GetAllNonFlagged();
    Response<IEnumerable<Message>> GetAll();
    Response<IEnumerable<Message>> GetAllFollowedByUser(string userId);
    Response<Message> Create(string userID, string text);
}