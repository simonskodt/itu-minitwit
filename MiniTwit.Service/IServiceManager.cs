using MiniTwit.Service.IServices;

namespace MiniTwit.Service;

public interface IServiceManager
{
    IUserService UserService { get; }
    IFollowerService FollowerService { get; }
    ILatestService LatestService { get; }
    IMessageService MessageService { get; }
    IAuthenticationService AuthenticationService { get; }
}
