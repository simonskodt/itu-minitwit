using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MiniTwit.Core.DTOs;
using MiniTwit.Core.Entities;
using MiniTwit.Core.Error;

namespace MiniTwit.Tests.Integration.Integrations;

public class TwitterTests : IClassFixture<CustomWebApplicationFactory>
{
    private CustomWebApplicationFactory _factory;

    public TwitterTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Timeline_given_valid_userId_returns_all_messages_from_followers_and_OK()
    {
        var client = _factory.CreateClient();
        var actual = await client.GetAsync("/?userId=000000000000000000000001");
        var content = await actual.Content.ReadFromJsonAsync<IEnumerable<Message>>();

        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Collection(content!,
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000006", AuthorId = "000000000000000000000002", Text = "Simon's third tweet", PubDate = DateTime.Parse("01/01/2023 12:00:04"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000005", AuthorId = "000000000000000000000002", Text = "Simon's second tweet", PubDate = DateTime.Parse("01/01/2023 12:00:03"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000004", AuthorId = "000000000000000000000002", Text = "Simon's first tweet", PubDate = DateTime.Parse("01/01/2023 12:00:02"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000001", AuthorId = "000000000000000000000001", Text = "Gustav's first tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000002", AuthorId = "000000000000000000000001", Text = "Gustav's second tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 })
        );
    }

    [Fact]
    public async Task Timeline_given_invalid_userId_returns_NotFound()
    {
        var actual = await _factory.CreateClient().GetAsync("/?userId=000000000000000000000000");

        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task Public_returns_all_non_flagged_messages_and_OK()
    {
        var actual = await _factory.CreateClient().GetAsync("/public/1");
        var content = await actual.Content.ReadFromJsonAsync<IEnumerable<Message>>();

        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Collection(content!,
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000006", AuthorId = "000000000000000000000002", Text = "Simon's third tweet", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000005", AuthorId = "000000000000000000000002", Text = "Simon's second tweet", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000004", AuthorId = "000000000000000000000002", Text = "Simon's first tweet", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000003", AuthorId = "000000000000000000000001", Text = "Gustav's Flagged", PubDate = DateTime.Parse("01/01/2023 12:00:01"), Flagged = 1 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000001", AuthorId = "000000000000000000000001", Text = "Gustav's first tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000002", AuthorId = "000000000000000000000001", Text = "Gustav's second tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 })
        );
    }

    [Fact]
    public async Task UserTimeline_given_valid_username_returns_all_users_messages_and_OK()
    {
        var actual = await _factory.CreateClient().GetAsync("/Gustav");
        var content = await actual.Content.ReadFromJsonAsync<IEnumerable<Message>>();

        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Collection(content!,
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000003", AuthorId = "000000000000000000000001", Text = "Gustav's Flagged", PubDate = DateTime.Parse("01/01/2023 12:00:01"), Flagged = 1 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000001", AuthorId = "000000000000000000000001", Text = "Gustav's first tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000002", AuthorId = "000000000000000000000001", Text = "Gustav's second tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000004", AuthorId = "000000000000000000000002", Text = "Simon's first tweet", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000005", AuthorId = "000000000000000000000002", Text = "Simon's second tweet", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000006", AuthorId = "000000000000000000000002", Text = "Simon's third tweet", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 })
        );
    }

    [Fact]
    public async Task UserTimeline_given_invalid_username_returns_NotFound()
    {
        var actual = await _factory.CreateClient().GetAsync("/test");

        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task FollowUser_given_valid_username_returns_NoContent()
    {
        var actual = await _factory.CreateClient().PostAsync("/Victor/follow?userId=000000000000000000000002", null);

        Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
    }

    [Fact]
    public async Task FollowUser_given_invalid_username_returns_NotFound()
    {
        var actual = await _factory.CreateClient().PostAsync("/test/follow?userId=000000000000000000000002", null);

        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task UnfollowUser_given_valid_username_returns_OK()
    {
        var actual = await _factory.CreateClient().DeleteAsync("/Victor/unfollow?userId=000000000000000000000002");
        Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
    }

    [Fact]
    public async Task UnfollowUser_given_invalid_username_returns_NotFound()
    {
        var actual = await _factory.CreateClient().DeleteAsync("/test/unfollow?userId=000000000000000000000002");

        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task AddMessage_given_valid_userId_returns_NoContent()
    {
        var actual = await _factory.CreateClient().PostAsync("/add_message?userId=000000000000000000000001&text=test", null);

        Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
    }

    [Fact]
    public async Task AddMessage_given_invalid_userId_returns_NotFound()
    {
        var actual = await _factory.CreateClient().PostAsync("/add_message?userId=000000000000000000000000&text=test", null);
        var content = actual.Content.ReadFromJsonAsync<Message>();

        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task Login_given_valid_username_and_password_returns_OK()
    {
        var loginDTO = new LoginDTO { Username = "Gustav", Password = "password" };
        var actual = await _factory.CreateClient().PostAsJsonAsync("/login", loginDTO);

        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
    }

    [Fact]
    public async Task Login_given_invalid_username_returns_Unauthorized()
    {
        var loginDTO = new LoginDTO { Username = "G", Password = "password" };
        var actual = await _factory.CreateClient().PostAsJsonAsync("/login", loginDTO);
        var content = await actual.Content.ReadFromJsonAsync<APIError>();

        Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        Assert.Equal(401, content!.Status);
        Assert.Equal("Invalid username", content.ErrorMsg);
    }

    [Fact]
    public async Task Login_given_invalid_password_returns_Unauthorized()
    {
        var loginDTO = new LoginDTO { Username = "Gustav", Password = "pass" };
        var actual = await _factory.CreateClient().PostAsJsonAsync("/login", loginDTO);
        var content = await actual.Content.ReadFromJsonAsync<APIError>();

        Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        Assert.Equal(401, content!.Status);
        Assert.Equal("Invalid password", content.ErrorMsg);
    }

    [Fact]
    public async Task Register_given_non_taken_username_returns_NoContent()
    {
        var registerDTO = new UserCreateDTO { Username = "The Tester", Email = "test@test.com", Password = "password" };
        var actual = await _factory.CreateClient().PostAsJsonAsync("/register", registerDTO);

        Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
    }

    [Fact]
    public async Task Register_given_taken_username_returns_BadRequest()
    {
        var registerDTO = new UserCreateDTO { Username = "Gustav", Email = "test@test.com", Password = "password" };
        var actual = await _factory.CreateClient().PostAsJsonAsync("/register", registerDTO);

        Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
    }

    [Fact]
    public async Task Logout_returns_OK()
    {
        var actual = await _factory.CreateClient().PostAsync("/logout", null);

        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
    }
}
