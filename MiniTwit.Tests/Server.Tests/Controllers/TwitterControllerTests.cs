using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniTwit.Core.DTOs;
using MiniTwit.Core.Error;
using MiniTwit.Core.Responses;
using MiniTwit.Server.Controllers;
using MiniTwit.Service;
using static MiniTwit.Core.Responses.HTTPResponse;

namespace MiniTwit.Tests.Server.Tests.Controllers;

public class TwitterControllerTests
{
    private Mock<IServiceManager> _manager;
    private Mock<ILogger<TwitterController>> _logger;
    private CancellationToken _ct;
    private HttpContext _HttpContext;

    public TwitterControllerTests()
    {
        _manager = new Mock<IServiceManager>();
        _logger = new Mock<ILogger<TwitterController>>();
        _ct = new CancellationToken();
        _HttpContext = new DefaultHttpContext
        {
            Connection =
            {
                RemoteIpAddress = new IPAddress(16885952)
            }
        };
    }

    [Fact]
    public async Task Timeline_given_valid_UserId_returns_OK()
    {
        // Arrange
        var expected = new Response<IEnumerable<MessageDTO>>(Ok, Array.Empty<MessageDTO>(), null);

        _manager.Setup(m => m.MessageService.GetAllFollowedByUserIdAsync("1", _ct)).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);
        controller.ControllerContext = new ControllerContext { HttpContext = _HttpContext };

        // Act
        var actual = (await controller.Timeline("1", _ct)).Result as OkObjectResult;

        //Assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals("We got a visitor from: 192.168.1.1", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once
        );

        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public async Task Timeline_given_invalid_UserId_returns_NotFound()
    {
        // Arrange
        var expected = new Response<IEnumerable<MessageDTO>>(NotFound, null, ErrorType.INVALID_USER_ID);
        var expectedAPIError = new APIError
        {
            Status = 404,
            ErrorMsg = "Invalid user id"
        };

        _manager.Setup(m => m.MessageService.GetAllFollowedByUserIdAsync("0", _ct)).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);
        controller.ControllerContext = new ControllerContext { HttpContext = _HttpContext };

        // Act
        var actual = (await controller.Timeline("0", _ct)).Result as NotFoundObjectResult;

        // Assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals("We got a visitor from: 192.168.1.1", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once
        );

        Assert.Equal(404, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task PublicTimeline_returns_OK()
    {
        // Arrange
        var expected = new Response<IEnumerable<MessageDTO>>(Ok, Array.Empty<MessageDTO>(), null);

        _manager.Setup(m => m.MessageService.GetAllNonFlaggedPageNumberLimitAsync(1, _ct)).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = (await controller.PublicTimeline(1, _ct)).Result as OkObjectResult;

        // Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public async Task UserTimeline_given_valid_username_returns_OK()
    {
        // Arrange
        var expected = new Response<IEnumerable<MessageDTO>>(Ok, Array.Empty<MessageDTO>(), null);

        _manager.Setup(m => m.MessageService.GetAllByUsernameAsync("test", _ct)).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = (await controller.UserTimeline("test")).Result as OkObjectResult;

        // Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public async Task UserTimeline_given_invalid_username_returns_NotFound()
    {
        // Arrange
        var expected = new Response<IEnumerable<MessageDTO>>(NotFound, null, ErrorType.INVALID_USERNAME);
        var expectedAPIError = new APIError
        {
            Status = 404,
            ErrorMsg = "Invalid username"
        };

        _manager.Setup(m => m.MessageService.GetAllByUsernameAsync("test", _ct)).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = (await controller.UserTimeline("test")).Result as NotFoundObjectResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task FollowUser_given_valid_userId_and_username_returns_NoContent()
    {
        // Arrange
        var expected = new Response(NoContent, null);

        _manager.Setup(m => m.FollowerService.CreateAsync("1", "test")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.FollowUser("test", "1") as NoContentResult;

        // Assert
        Assert.Equal(204, actual!.StatusCode);
    }

    [Fact]
    public async Task FollowUser_given_valid_userId_and_invalid_username_returns_NotFound()
    {
        // Arrange
        var expected = new Response(NotFound, ErrorType.INVALID_USERNAME);
        var expectedAPIError = new APIError
        {
            Status = 404,
            ErrorMsg = "Invalid username"
        };

        _manager.Setup(m => m.FollowerService.CreateAsync("1", "test")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.FollowUser("test", "1") as NotFoundObjectResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task FollowUser_given_invalid_userId_and_valid_username_returns_NotFound()
    {
        // Arrange
        var expected = new Response(NotFound, ErrorType.INVALID_USER_ID);
        var expectedAPIError = new APIError
        {
            Status = 404,
            ErrorMsg = "Invalid user id"
        };

        _manager.Setup(m => m.FollowerService.CreateAsync("1", "test")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.FollowUser("test", "1") as NotFoundObjectResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task UnfollowerUser_given_valid_userId_and_username_returns_NoContent()
    {
        // Arrange
        var expected = new Response(NoContent, null);

        _manager.Setup(m => m.FollowerService.DeleteAsync("1", "test")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.UnfollowUser("test", "1") as NoContentResult;

        // Assert
        Assert.Equal(204, actual!.StatusCode);
    }

    [Fact]
    public async Task UnfollowerUser_given_valid_userId_and_invalid_username_returns_NotFound()
    {
        // Arrange
        var expected = new Response(NotFound, ErrorType.INVALID_USERNAME);
        var expectedAPIError = new APIError
        {
            Status = 404,
            ErrorMsg = "Invalid username"
        };

        _manager.Setup(m => m.FollowerService.DeleteAsync("1", "test")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.UnfollowUser("test", "1") as NotFoundObjectResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task UnfollowerUser_given_invalid_userId_and_valid_username_returns_NotFound()
    {
        // Arrange
        var expected = new Response(NotFound, ErrorType.INVALID_USER_ID);
        var expectedAPIError = new APIError
        {
            Status = 404,
            ErrorMsg = "Invalid user id"
        };

        _manager.Setup(m => m.FollowerService.DeleteAsync("1", "test")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.UnfollowUser("test", "1") as NotFoundObjectResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task AddMessage_given_valid_userId_returns_NotFound()
    {
        // Arrange
        var expected = new Response(NoContent, null);

        _manager.Setup(m => m.MessageService.CreateAsync("1", "text")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.AddMessage("1", "text") as NoContentResult;

        // Assert
        Assert.Equal(204, actual!.StatusCode);
    }

    [Fact]
    public async Task AddMessage_given_invalid_userId_returns_NotFound()
    {
        // Arrange
        var expected = new Response(NotFound, ErrorType.INVALID_USER_ID);
        var expectedAPIError = new APIError
        {
            Status = 404,
            ErrorMsg = "Invalid user id"
        };

        _manager.Setup(m => m.MessageService.CreateAsync("1", "text")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.AddMessage("1", "text") as NotFoundObjectResult;

        // Assert
        Assert.Equal(404, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task Login_given_valid_username_and_password_returns_OK()
    {
        // Arrange
        var expected = new Response<UserDTO>(Ok, new UserDTO { Id = "1", Username = "test", Email = "test@test.com" });

        var loginDTO = new LoginDTO { Username = "test", Password = "password" };

        _manager.Setup(m => m.AuthenticationService.AuthenticateAsync("test", "password")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = (await controller.Login(loginDTO)).Result as OkObjectResult;

        // Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    [Fact]
    public async Task Login_given_invalid_username_returns_Unauthorized()
    {
        // Arrange
        var expected = new Response<UserDTO>(Unauthorized, null, ErrorType.INVALID_USERNAME);
        var expectedAPIError = new APIError
        {
            Status = 401,
            ErrorMsg = "Invalid username"
        };

        _manager.Setup(m => m.AuthenticationService.AuthenticateAsync("test", "password")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = (await controller.Login(new LoginDTO { Username = "test", Password = "password" })).Result as UnauthorizedObjectResult;

        // Assert
        Assert.Equal(401, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task Login_given_valid_username_and_invalid_password_returns_Unauthorized()
    {
        // Arrange
        var expected = new Response<UserDTO>(Unauthorized, null, ErrorType.INVALID_PASSWORD);
        var expectedAPIError = new APIError
        {
            Status = 401,
            ErrorMsg = "Invalid password"
        };

        _manager.Setup(m => m.AuthenticationService.AuthenticateAsync("test", "password")).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = (await controller.Login(new LoginDTO { Username = "test", Password = "password" })).Result as UnauthorizedObjectResult;

        // Assert
        Assert.Equal(401, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task Register_given_non_taken_username_returns_NoContent()
    {
        // Arrange
        var expected = new Response(NoContent, null);

        _manager.Setup(m => m.UserService.CreateAsync(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" })).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.Register(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" }) as NoContentResult;

        // Assert
        Assert.Equal(204, actual!.StatusCode);
    }

    [Fact]
    public async Task Register_given_taken_username_returns_Badrequest()
    {
        // Arrange
        var expected = new Response(BadRequest, ErrorType.USERNAME_TAKEN);
        var expectedAPIError = new APIError
        {
            Status = 400, 
            ErrorMsg = "The username is already taken" 
        };

        _manager.Setup(m => m.UserService.CreateAsync(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" })).ReturnsAsync(expected);

        var controller = new TwitterController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.Register(new UserCreateDTO { Email = "test@test.com", Username = "test", Password = "password" }) as BadRequestObjectResult;

        // Assert
        Assert.Equal(400, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    [Fact]
    public async Task Logout_returns_OK()
    {
        var controller = new TwitterController(_manager.Object, _logger.Object);
        var actual = await controller.Logout() as OkResult;

        Assert.Equal(200, actual!.StatusCode);
    }
}
