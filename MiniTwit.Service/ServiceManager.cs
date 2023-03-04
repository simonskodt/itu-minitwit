using MiniTwit.Core.IRepositories;
using MiniTwit.Security;
using MiniTwit.Service.IServices;
using MiniTwit.Service.Services;

namespace MiniTwit.Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserService> _lazyUserService;
    private readonly Lazy<IFollowerService> _lazyFollowerService;
    private readonly Lazy<ILatestService> _lazyLatestService;
    private readonly Lazy<IMessageService> _lazyMessageService;
    private readonly Lazy<IAuthenticationService> _lazyAuthenticationService;

    public IUserService UserService => _lazyUserService.Value;
    public IFollowerService FollowerService => _lazyFollowerService.Value;
    public ILatestService LatestService => _lazyLatestService.Value;
    public IMessageService MessageService => _lazyMessageService.Value;
    public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;

    public ServiceManager(IUserRepository userRepository, IFollowerRepository followerRepository, ILatestRepository latestRepository, IMessageRepository messageRepository, IHasher hasher)
    {
        _lazyUserService = new Lazy<IUserService>(() => new UserService(userRepository, hasher));
        _lazyFollowerService = new Lazy<IFollowerService>(() => new FollowerService(followerRepository));
        _lazyLatestService = new Lazy<ILatestService>(() => new LatestService(latestRepository));
        _lazyMessageService = new Lazy<IMessageService>(() => new MessageService(messageRepository, userRepository));
        _lazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userRepository, hasher));
    }
}
