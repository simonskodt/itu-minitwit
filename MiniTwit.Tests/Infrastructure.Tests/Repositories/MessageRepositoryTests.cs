using FluentAssertions;
using FluentAssertions.Extensions;
using MiniTwit.Core.Responses;
using MiniTwit.Core.Error;

namespace MiniTwit.Tests.Infrastructure.Repositories;

public class MessageRepositoryTests : RepoTests
{
    private readonly MessageRepository _repository;

    public MessageRepositoryTests()
    {
        _repository = new MessageRepository(_context);
    }

    [Fact]
    public void Create_given_eixsting_userid_creates_new_message_and_returns_message_with_no_error()
    {
        var message = new Message
        {
            AuthorId = "000000000000000000000001",
            PubDate = DateTime.Now,
            Flagged = 0,
            Text = "Test Tweet"
        };

        var expected = new DBResult<Message>
        {
            Model = message,
            ErrorType = null
        };

        var actual = _repository.Create("000000000000000000000001", "Test Tweet");

        Assert.Equal(expected.ErrorType, actual.ErrorType);
        Assert.Equal(expected.Model.AuthorId, actual.Model!.AuthorId);
        Assert.Equal(expected.Model.Text, actual.Model!.Text);
        actual.Model.PubDate.Should().BeCloseTo(expected.Model.PubDate, 2000.Milliseconds());
        Assert.Equal(expected.Model.Flagged, actual.Model!.Flagged);
    }

    [Fact]
    public void Create_given_non_existing_userid_returns_ErrorType_INVALID_USER_ID()
    {
        var actual = _repository.Create("000000000000000000000000", "Test Tweet");

        Assert.Equal(ErrorType.INVALID_USER_ID, actual.ErrorType);
        Assert.Null(actual.Model);
    }

    [Fact]
    public void GetAllNonFlagged_returns_all_non_flagged_messages_in_PubDate_descending_order_with_no_error()
    {
        var actual = _repository.GetAllNonFlagged();

        Assert.Null(actual.ErrorType);
        Assert.Collection(actual.Model!,
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

    [Fact]
    public void GetAllByUserId_given_existing_UserId_returns_all_their_messages_in_PubDate_descending_order_with_no_error()
    {
        var actual = _repository.GetAllByUserId("000000000000000000000001");

        Assert.Null(actual.ErrorType);
        Assert.Collection(actual.Model!,
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000003", AuthorId = "000000000000000000000001", Text = "Gustav's Flagged", PubDate = DateTime.Parse("01/01/2023 12:00:01"), Flagged = 1 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000001", AuthorId = "000000000000000000000001", Text = "Gustav's first tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000002", AuthorId = "000000000000000000000001", Text = "Gustav's second tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 })
        );
    }

    [Fact]
    public void GetAllByUserId_given_non_existing_UserId_returns_error_INVALID_USER_ID()
    {
        var actual = _repository.GetAllByUserId("000000000000000000000000");

        Assert.Equal(ErrorType.INVALID_USER_ID, actual.ErrorType);
        Assert.Null(actual.Model);
    }

    [Fact]
    public void GetAllByUsername_given_existing_username_returns_all_their_messages_in_PubDate_descending_order_with_no_error()
    {
        var actual = _repository.GetAllByUsername("Gustav");

        Assert.Null(actual.ErrorType);
        Assert.Collection(actual.Model!,
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000003", AuthorId = "000000000000000000000001", Text = "Gustav's Flagged", PubDate = DateTime.Parse("01/01/2023 12:00:01"), Flagged = 1 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000001", AuthorId = "000000000000000000000001", Text = "Gustav's first tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000002", AuthorId = "000000000000000000000001", Text = "Gustav's second tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 })
        );
    }

    [Fact]
    public void GetAllByUsername_given_non_existing_username_returns_error_INVALID_USERNAME()
    {
        var actual = _repository.GetAllByUsername("test");

        Assert.Equal(ErrorType.INVALID_USERNAME, actual.ErrorType);
        Assert.Null(actual.Model);
    }

    [Fact]
    public void GetAllFollowedByUser_given_existing_UserId_returns_non_flagged_messages_from_followed_users_in_PubDate_descending_order_with_no_error()
    {
        var actual = _repository.GetAllFollowedByUserId("000000000000000000000001");

        Assert.Null(actual.ErrorType);
        Assert.Collection(actual.Model!,
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000006", AuthorId = "000000000000000000000002", Text = "Simon's third tweet", PubDate = DateTime.Parse("01/01/2023 12:00:04"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000005", AuthorId = "000000000000000000000002", Text = "Simon's second tweet", PubDate = DateTime.Parse("01/01/2023 12:00:03"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000004", AuthorId = "000000000000000000000002", Text = "Simon's first tweet", PubDate = DateTime.Parse("01/01/2023 12:00:02"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000001", AuthorId = "000000000000000000000001", Text = "Gustav's first tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 }),
            m => m.Should().BeEquivalentTo(new Message { Id = "000000000000000000000002", AuthorId = "000000000000000000000001", Text = "Gustav's second tweet!", PubDate = DateTime.Parse("01/01/2023 12:00:00"), Flagged = 0 })
        );
    }

    [Fact]
    public void GetAllFollowedByUser_given_non_existing_UserId_returns_errortype_INVALID_USERID()
    {
        var actual = _repository.GetAllFollowedByUserId("000000000000000000000000");

        Assert.Equal(ErrorType.INVALID_USER_ID, actual.ErrorType);
        Assert.Null(actual.Model);
    }
}
