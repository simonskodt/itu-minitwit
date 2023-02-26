using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MiniTwit.Core.Entities;

namespace MiniTwit.Tests.Integration.Integrations;

public class TwitterTests : IClassFixture<CustomWebApplicationFactory>
{
    private HttpClient _client;

    public TwitterTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Timeline_given_valid_userId_returns_all_messages_from_followers_and_OK()
    {
        var actual = await _client.GetAsync("/?userId=000000000000000000000001");

        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        var content = await actual.Content.ReadFromJsonAsync<IEnumerable<Message>>();

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
        var actual = await _client.GetAsync("/?userId=000000000000000000000000");

        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task Public_returns_all_non_flagged_messages_and_OK()
    {
        var actual = await _client.GetAsync("/public");

        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        var content = await actual.Content.ReadFromJsonAsync<IEnumerable<Message>>();

        Assert.Collection(content!,
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000008", AuthorId = "000000000000000000000003", Text = "Nikolaj2", PubDate = DateTime.Parse("01/01/2023 12:00:06"), Flagged = 0 }),              // 1
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000007", AuthorId = "000000000000000000000003", Text = "Nikolaj1", PubDate = DateTime.Parse("01/01/2023 12:00:05"), Flagged = 0 }),              // 2
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000006", AuthorId = "000000000000000000000002", Text = "Simon's third tweet", PubDate = DateTime.Parse("01/01/2023 12:00:04"), Flagged = 0 }),   // 3
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000005", AuthorId = "000000000000000000000002", Text = "Simon's second tweet", PubDate = DateTime.Parse("01/01/2023 12:00:03"), Flagged = 0 }),  // 4
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000004", AuthorId = "000000000000000000000002", Text = "Simon's first tweet", PubDate = DateTime.Parse("01/01/2023 12:00:02"), Flagged = 0 }),   // 5
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000010", AuthorId = "000000000000000000000004", Text = "Victor2", PubDate = DateTime.Parse("01/01/2023 12:00:02"), Flagged = 0 }),               // 6
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000009", AuthorId = "000000000000000000000004", Text = "Victor1", PubDate = DateTime.Parse("01/01/2023 12:00:01"), Flagged = 0 }),               // 7
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000001", AuthorId = "000000000000000000000001", Text = "Gustav's first tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }), // 8
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000002", AuthorId = "000000000000000000000001", Text = "Gustav's second tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }) // 9
        );
    }
}
