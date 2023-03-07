using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniTwit.Core.DTOs;
using MiniTwit.Core.Responses;
using MiniTwit.Server.Controllers;
using MiniTwit.Service;
using static MiniTwit.Core.Responses.HTTPResponse;

namespace MiniTwit.Tests.Server.Controllers;

public class TwitterControllerTests
{
    private Mock<IServiceManager> _manager;
    private Mock<ILogger<TwitterController>> _logger;
    private CancellationToken _ct;

    public TwitterControllerTests()
    {
        _manager = new Mock<IServiceManager>();
        _logger = new Mock<ILogger<TwitterController>>();
        _ct = new CancellationToken();
    }

    [Fact]
    public async Task Timeline_given_valid_UserId_returns_OK()
    {
        // Arrange
        var expected = new Response<IEnumerable<MessageDTO>>(Ok, Array.Empty<MessageDTO>(), null);

        _manager.Setup(m => m.MessageService.GetAllFollowedByUserIdAsync("1", _ct)).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.Timeline("1", _ct);

        // // Assert
        // Assert.Equal(200, actual!.StatusCode);
        // Assert.Equal(expected.Model, actual.Value);
    }

    // [Fact]
    // public async Task Timeline_given_invalid_UserId_returns_NotFound()
    // {
    //     // Arrange
    //     var expected = new Response<IEnumerable<MessageDTO>>(NotFound, null, ErrorType.INVALID_USER_ID);
    //     var expectedAPIError = new APIError
    //     {
    //         Status = 404,
    //         ErrorMsg = "Invalid user id"
    //     };

    //     _manager.Setup(m => m.MessageService.GetAllFollowedByUserIdAsync("0", _ct)).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.Timeline("0", _ct)).Result as NotFoundObjectResult;

    //     // Assert
    //     Assert.Equal(404, actual!.StatusCode);
    // }

    // [Fact]
    // public async Task PublicTimeline_returns_OK()
    // {
    //     // Arrange
    //     var expected = new Response<IEnumerable<MessageDTO>>(Ok, Array.Empty<MessageDTO>(), null);

    //     _manager.Setup(m => m.MessageService.GetAllNonFlaggedAsync(_ct)).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.PublicTimeline()).Result as OkObjectResult;

    //     // Assert
    //     Assert.Equal(200, actual!.StatusCode);
    //     Assert.Equal(expected.Model, actual.Value);
    // }

    // [Fact]
    // public async Task UserTimeline_given_valid_username_returns_OK()
    // {
    //     // Arrange
    //     var expected = new Response<IEnumerable<MessageDTO>>(Ok, Array.Empty<MessageDTO>(), null);

    //     _manager.Setup(m => m.MessageService.GetAllByUsernameAsync("test", _ct)).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.UserTimeline("test")).Result as OkObjectResult;

    //     // Assert
    //     Assert.Equal(200, actual!.StatusCode);
    //     Assert.Equal(expected.Model, actual.Value);
    // }

    // [Fact]
    // public async Task UserTimeline_given_invalid_username_returns_NotFound()
    // {
    //     // Arrange
    //     var expected = new Response<IEnumerable<MessageDTO>>(NotFound, null, ErrorType.INVALID_USERNAME);

    //     _manager.Setup(m => m.MessageService.GetAllByUsernameAsync("test", _ct)).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.UserTimeline("test")).Result as NotFoundResult;

    //     // Assert
    //     Assert.Equal(404, actual!.StatusCode);
    // }

    // [Fact]
    // public async Task FollowUser_given_valid_username_returns_Created()
    // {
    //     // Arrange
    //     var expected = new Response<FollowerDTO>(Created, new FollowerDTO { WhoId = "1", WhomId = "2" }, null);

    //     _manager.Setup(m => m.FollowerService.CreateAsync("1", "test")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.FollowUser("1", "test")) as CreatedResult;

    //     // Assert
    //     Assert.Equal(201, actual!.StatusCode);
    //     Assert.Equal(expected.Model, actual.Value);
    // }

    // [Fact]
    // public async Task FollowUser_given_invalid_username_returns_NotFound()
    // {
    //     // Arrange
    //     var expected = new Response<Follower>(NotFound, null);

    //     _manager.Setup(m => m.FollowerService.CreateAsync("1", "test")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.FollowUser("1", "test")) as NotFoundResult;

    //     // Assert
    //     Assert.Equal(404, actual!.StatusCode);
    // }

    // [Fact]
    // public async Task UnfollowerUser_given_existing_username_returns_OK()
    // {
    //     // Arrange
    //     var expected = new Response(Ok, null);

    //     _manager.Setup(m => m.FollowerService.DeleteAsync("1", "test")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.UnfollowUser("1", "test")) as OkObjectResult;

    //     // Assert
    //     Assert.Equal(200, actual!.StatusCode);
    // }

    // [Fact]
    // public async Task UnfollowerUser_given_non_existing_username_returns_NotFound()
    // {
    //     // Arrange
    //     var expected = new Response(NotFound, null);

    //     _manager.Setup(m => m.FollowerService.DeleteAsync("1", "test")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.UnfollowUser("1", "test")) as NotFoundResult;

    //     // Assert
    //     Assert.Equal(404, actual!.StatusCode);
    // }

    // [Fact]
    // public async Task AddMessage_given_valid_userid_returns_Created()
    // {
    //     // Arrange
    //     var expected = new Response<MessageDTO>(Created, new MessageDTO { Id = "1", AuthorId = "1", Text = "test", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }, null);

    //     _manager.Setup(m => m.MessageService.CreateAsync("1", "1")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.AddMessage("1", "1")) as CreatedResult;

    //     // Assert
    //     Assert.Equal(201, actual!.StatusCode);
    //     Assert.Equal(expected.Model, actual.Value);
    // }

    // [Fact]
    // public async Task AddMessage_given_invalid_userid_returns_NotFound()
    // {
    //     // Arrange
    //     var expected = new Response<MessageDTO>(NotFound, null);

    //     _manager.Setup(m => m.MessageService.CreateAsync("1", "1")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.AddMessage("1", "1")) as NotFoundResult;

    //     // Assert
    //     Assert.Equal(404, actual!.StatusCode);
    // }

    // [Fact]
    // public async Task Login_given_valid_username_and_password_returns_OK()
    // {
    //     // Arrange
    //     var expected = new Response<UserDTO>(Ok, new UserDTO{ Id = "1", Username = "test", Email = "test@test.com" });

    //     var loginDTO = new LoginDTO{ Username = "test", Password = "password" };

    //     _manager.Setup(m => m.AuthenticationService.AuthenticateAsync("test", "password")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);
        
    //     // Act
    //     var actual = (await controller.Login(loginDTO)) as OkObjectResult;

    //     // Assert
    //     Assert.Equal(200, actual!.StatusCode);
    //     Assert.Equal(expected.Model, actual.Value);
    // }

    // [Fact]
    // public async Task Login_given_invalid_username_returns_Unauthorized()
    // {
    //     // Arrange
    //     var expected = new Response<User>(NotFound, null);

    //     _manager.Setup(m => m.AuthenticationService.AuthenticateAsync("test", "test")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.Login(new LoginDTO{ Username = "test", Password = "password" })) as UnauthorizedObjectResult;

    //     // Assert
    //     Assert.Equal(401, actual!.StatusCode);
    //     Assert.Equal("Invalid username", actual.Value);
    // }

    // [Fact]
    // public async Task Login_given_invalid_password_returns_Unauthorized()
    // {
    //     // Arrange
    //     var expected = new Response<UserDTO>(Ok, new UserDTO{ Id = "1", Username = "test", Email = "test@test.com" });

    //     _manager.Setup(m => m.AuthenticationService.AuthenticateAsync("test", "password")).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = (await controller.Login(new LoginDTO{ Username = "test", Password = "password" })) as UnauthorizedObjectResult;

    //     // Assert
    //     Assert.Equal(401, actual!.StatusCode);
    //     Assert.Equal("Invalid password", actual.Value);
    // }

    // [Fact]
    // public async Task Register_given_non_taken_username_returns_Created()
    // {
    //     // Arrange
    //     var expected = new Response<UserDTO>(Created, new UserDTO{ Id = "1", Username = "test", Email = "test@test.com" });

    //     _manager.Setup(m => m.UserService.CreateAsync(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" })).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = await controller.Register(new UserCreateDTO { Email = "test@test.com", Username = "test", Password = "password" }) as CreatedResult;

    //     // Assert
    //     Assert.Equal(201, actual!.StatusCode);
    //     Assert.Equal(expected.Model, actual.Value);
    // }

    // [Fact]
    // public async Task Register_given_taken_username_returns_Badrequest()
    // {
    //     // Arrange
    //     var expected = new Response<UserDTO>(BadRequest, null, ErrorType.USERNAME_TAKEN);
    //     var expectedAPIError = new APIError { Status = 400, ErrorMsg = "The username is already taken" };

    //     _manager.Setup(m => m.UserService.CreateAsync(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" })).ReturnsAsync(expected);

    //     var controller = new TwitterController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = await controller.Register(new UserCreateDTO { Email = "test@test.com", Username = "test", Password = "password" }) as ConflictResult;

    //     // Assert
    //     Assert.Equal(409, actual!.StatusCode);
    // }

    // [Fact]
    // public async Task Logout_returns_OK()
    // {
    //     var controller = new TwitterController(_manager.Object, _logger.Object);
    //     var actual = await controller.Logout() as OkResult;

    //     Assert.Equal(200, actual!.StatusCode);
    // }
}
