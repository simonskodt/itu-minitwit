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

public class SimControllerTests
{
    private Mock<IServiceManager> _manager;
    private Mock<ILogger<SimController>> _logger;
    private CancellationToken _ct;
    private HttpContext _HttpContext;

    public SimControllerTests()
    {
        _manager = new Mock<IServiceManager>();
        _logger = new Mock<ILogger<SimController>>();
        _ct = new CancellationToken();
        _HttpContext = new DefaultHttpContext
        {
            Connection =
            {
                RemoteIpAddress = new IPAddress(16885952)
            }
        };
    }

    public async Task Latest_returns_OK()
    {
        // Arrange
        var expected = new Response<LatestDTO>(Ok, new LatestDTO { LatestVal = 100 }, null);

        _manager.Setup(m => m.LatestService.GetAsync(_ct)).ReturnsAsync(expected);

        var controller = new SimController(_manager.Object, _logger.Object);

        // Act
        var actual = (await controller.Latest(_ct)).Result as OkObjectResult;

        //Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expected.Model, actual.Value);
    }

    public async Task Register_given_non_taken_username_returns_NoContent()
    {
         // Arrange
        var expected = new Response(NoContent, null);

        _manager.Setup(m => m.UserService.CreateAsync(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" })).ReturnsAsync(expected);

        var controller = new SimController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.Register(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" }) as NoContentResult;

        //Assert
        Assert.Equal(204, actual!.StatusCode);
    }

    public async Task Register_given_taken_username_returns_BadRequest()
    {
        // Arrange
        var expected = new Response(BadRequest, ErrorType.USERNAME_TAKEN);
        var expectedAPIError = new APIError
        {
            Status = 400,
            ErrorMsg = "The username is already taken"
        };

        _manager.Setup(m => m.UserService.CreateAsync(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" })).ReturnsAsync(expected);

        var controller = new SimController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.Register(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" }) as BadRequestObjectResult;

        //Assert
        Assert.Equal(200, actual!.StatusCode);
        Assert.Equal(expectedAPIError, actual.Value);
    }

    // public async Task Msgs_when_authorized_returns_OK()
    // {
    //     // Arrange
    //     var expected = new Response<IEnumerable<MessageDetailsDTO>>(BadRequest, Array.Empty<MessageDetailsDTO>(), null);

    //     _manager.Setup(m => m.MessageService.GetAllNonFlaggedAsync(_ct)).ReturnsAsync(expected);

    //     var controller = new SimController(_manager.Object, _logger.Object);

    //     // Act
    //     var actual = await controller.Register(new UserCreateDTO { Username = "test", Email = "test@test.com", Password = "password" }) as BadRequestObjectResult;

    //     //Assert
    //     Assert.Equal(200, actual!.StatusCode);
    //     Assert.Equal(expectedAPIError, actual.Value);
    // }

    public async Task PostMsgUsername_when_authorized_returns_NoContent()
    {
        // Arrange
        var expected = new Response(NoContent, null);

        _manager.Setup(m => m.MessageService.CreateAsync("test", "text")).ReturnsAsync(expected);

        var controller = new SimController(_manager.Object, _logger.Object);

        // Act
        var actual = await controller.PostMsgUsername("test", new MessageCreateDTO { Content = "text" }) as NoContentResult;

        //Assert
        Assert.Equal(204, actual!.StatusCode);
    }
}
