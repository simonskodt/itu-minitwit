using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MiniTwit.Security;
using MiniTwit.Server.Controllers;

namespace MiniTwit.Tests.Server.Controllers;

public class TwitterControllerTests
{
    private Mock<IUserRepository> _userRepository;
    private Mock<IMessageRepository> _messageRepository;
    private Mock<IFollowerRepository> _followerRepository;
    private Mock<IHasher> _hasher;

    public TwitterControllerTests()
    {
        _messageRepository = new Mock<IMessageRepository>();
        _userRepository = new Mock<IUserRepository>();
        _followerRepository = new Mock<IFollowerRepository>();
        _hasher = new Mock<IHasher>();
    }

    [Fact]
    public void Timeline_given_valid_UserId_returns_OK()
    {
        // Arrange
        var expected = new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = Array.Empty<Message>()
        };

        _messageRepository.Setup(r => r.GetAllFollowedByUser("1")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.Timeline("1") as OkObjectResult;

        // Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public void Timeline_given_invalid_UserId_returns_NotFound()
    {
        // Arrange
        var expected = new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.NotFound,
            Model = null
        };

        _messageRepository.Setup(r => r.GetAllFollowedByUser("0")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.Timeline("0") as NotFoundResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
    }

    [Fact]
    public void PublicTimeline_returns_OK()
    {
        // Arrange
        var expected = new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = Array.Empty<Message>()
        };

        _messageRepository.Setup(r => r.GetAllNonFlagged()).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.PublicTimeline() as OkObjectResult;

        // Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public void UserTimeline_given_valid_username_returns_OK()
    {
        // Arrange
        var expected = new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = Array.Empty<Message>()
        };

        _messageRepository.Setup(r => r.GetAllByUsername("test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.UserTimeline("test") as OkObjectResult;

        // Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public void UserTimeline_given_invalid_username_returns_NotFound()
    {
        // Arrange
        var expected = new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.NotFound,
            Model = null
        };

        _messageRepository.Setup(r => r.GetAllByUsername("test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.UserTimeline("test") as NotFoundResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
    }

    [Fact]
    public void FollowUser_given_valid_username_returns_Created()
    {
        // Arrange
        var expected = new Response<Follower>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = new Follower{ WhoId = "1", WhomId = "2" }
        };

        _followerRepository.Setup(r => r.Create("1", "test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.FollowUser("1", "test") as CreatedResult;

        // Assert
        Assert.Equal(201, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public void FollowUser_given_invalid_username_returns_NotFound()
    {
        // Arrange
        var expected = new Response<Follower>
        {
            HTTPResponse = HTTPResponse.NotFound,
            Model = null
        };

        _followerRepository.Setup(r => r.Create("1", "test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.FollowUser("1", "test") as NotFoundResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
    }

    [Fact]
    public void UnfollowerUser_given_existing_username_returns_OK()
    {
        // Arrange
        var expected = new Response
        {
            HTTPResponse = HTTPResponse.Success
        };

        _followerRepository.Setup(r => r.Delete("1", "test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.UnfollowUser("1", "test") as OkObjectResult;

        // Assert
        Assert.Equal(200, actual!.StatusCode);
    }

    [Fact]
    public void UnfollowerUser_given_non_existing_username_returns_NotFound()
    {
        // Arrange
        var expected = new Response
        {
            HTTPResponse = HTTPResponse.NotFound
        };

        _followerRepository.Setup(r => r.Delete("1", "test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.UnfollowUser("1", "test") as NotFoundResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
    }

    [Fact]
    public void AddMessage_given_valid_userid_returns_Created()
    {
        // Arrange
        var expected = new Response<Message>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = new Message { Id = "1", AuthorId = "1", Text = "test", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }
        };

        _messageRepository.Setup(r => r.Create("1", "1")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.AddMessage("1", "1") as CreatedResult;

        // Assert
        Assert.Equal(201, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public void AddMessage_given_invalid_userid_returns_NotFound()
    {
        // Arrange
        var expected = new Response<Message>
        {
            HTTPResponse = HTTPResponse.NotFound,
            Model = null
        };

        _messageRepository.Setup(r => r.Create("1", "1")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.AddMessage("1", "1") as NotFoundResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
    }

    [Fact]
    public void Login_given_valid_username_and_password_returns_OK()
    {
        // Arrange
        var expected = new Response<User>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = new User{ Id = "1", Username = "test", Email = "test@test.com", Password = "password" }
        };

        var loginDTO = new LoginDTO{ Username = "test", Password = "password" };

        _hasher.Setup(h => h.VerifyHash(loginDTO.Password, expected.Model.Password)).Returns(true);
        _userRepository.Setup(r => r.GetByUsername("test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);
        
        // Act
        var actual = controller.Login(loginDTO) as OkObjectResult;

        // Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public void Login_given_invalid_username_returns_Unauthorized()
    {
        // Arrange
        var expected = new Response<User>
        {
            HTTPResponse = HTTPResponse.NotFound,
            Model = null
        };

        _hasher.Setup(h => h.VerifyHash("password", "password")).Returns(true);
        _userRepository.Setup(r => r.GetByUsername("test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.Login(new LoginDTO{ Username = "test", Password = "password" }) as UnauthorizedObjectResult;

        // Assert
        Assert.Equal(401, actual!.StatusCode);
        Assert.Equal("Invalid username", actual.Value);
    }

    [Fact]
    public void Login_given_invalid_password_returns_Unauthorized()
    {
        // Arrange
        var expected = new Response<User>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = new User{ Id = "1", Username = "test", Email = "test@test.com", Password = "password" }
        };

        _hasher.Setup(h => h.VerifyHash("password", "password")).Returns(false);
        _userRepository.Setup(r => r.GetByUsername("test")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.Login(new LoginDTO{ Username = "test", Password = "password" }) as UnauthorizedObjectResult;

        // Assert
        Assert.Equal(401, actual!.StatusCode);
        Assert.Equal("Invalid password", actual.Value);
    }

    [Fact]
    public void Register_given_non_taken_username_returns_Created()
    {
        // Arrange
        var expected = new Response<User>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = new User{ Id = "1", Username = "test", Email = "test@test.com", Password = "password" }
        };

        var password = "password";
        _hasher.Setup(h => h.Hash("password", out password));
        _userRepository.Setup(r => r.Create("test", "test@test.com", "password")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.Register(new RegisterDTO{ Email = "test@test.com", Username = "test", Password = "password" }) as CreatedResult;

        // Assert
        Assert.Equal(201, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public void Register_given_taken_username_returns_Conflict()
    {
        // Arrange
        var expected = new Response<User>
        {
            HTTPResponse = HTTPResponse.Conflict,
            Model = null
        };

        var password = "password";
        _hasher.Setup(h => h.Hash("password", out password));
        _userRepository.Setup(r => r.Create("test", "test@test.com", "password")).Returns(expected);

        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);

        // Act
        var actual = controller.Register(new RegisterDTO{ Email = "test@test.com", Username = "test", Password = "password" }) as ConflictResult;

        // Assert
        Assert.Equal(409, actual!.StatusCode);
    }

    [Fact]
    public void Logout_returns_OK()
    {
        var controller = new TwitterController(_hasher.Object, _userRepository.Object, _messageRepository.Object, _followerRepository.Object);
        var actual = controller.Logout() as OkResult;

        Assert.Equal(200, actual!.StatusCode);
    }
}
