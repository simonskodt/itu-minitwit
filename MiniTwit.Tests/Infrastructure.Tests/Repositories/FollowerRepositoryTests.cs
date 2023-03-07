using MiniTwit.Core.Responses;
using MongoDB.Driver;
using MiniTwit.Core.Error;

namespace MiniTwit.Tests.Infrastructure.Repositories;

public class FollowerRepositoryTests : RepoTests
{
    private readonly FollowerRepository _repository;

    public FollowerRepositoryTests()
    {
        _repository = new FollowerRepository(_context);
    }

    [Fact]
    public void Create_given_existing_target_username_creates_new_follower_and_returns_Created_with_no_error()
    {
        var follower = new Follower
        {
            WhoId = "000000000000000000000001",
            WhomId = "000000000000000000000004"
        };

        var expected = new DBResult<Follower>
        {
            ErrorType = null,
            Model = follower
        };

        var actual = _repository.Create("000000000000000000000001", "Victor");

        Assert.Equal(expected.ErrorType, actual.ErrorType);
        Assert.Equal("000000000000000000000001", actual.Model!.WhoId);
        Assert.Equal("000000000000000000000004", actual.Model!.WhomId);
    }

    [Fact]
    public void Create_given_non_existing_target_username_returns_errortype_INVALID_USERNAME()
    {
        var actual = _repository.Create("000000000000000000000001", "test");

        Assert.Equal(ErrorType.INVALID_USERNAME, actual.ErrorType);
        Assert.Null(actual.Model);
    }

    [Fact]
    public void Delete_given_existing_targetUsername_deletes_user_with_no_error()
    {
        var existing = _context.Followers.Find(f => f.WhoId == "000000000000000000000001" && f.WhomId == "000000000000000000000002").FirstOrDefault();

        Assert.NotNull(existing);
        Assert.Equal("000000000000000000000001", existing.WhoId);
        Assert.Equal("000000000000000000000002", existing.WhomId);

        var actual = _repository.Delete("000000000000000000000001", "Simon");

        Assert.Equal(null, actual.ErrorType);
        
        existing = _context.Followers.Find(f => f.WhoId == "000000000000000000000001" && f.WhomId == "000000000000000000000002").FirstOrDefault();

        Assert.Null(existing);
    }

    [Fact]
    public void Delete_given_non_existing_targetUsername_returns_errortype_INVALID_USERNAME()
    {
        var actual = _repository.Delete("000000000000000000000001", "test");

        Assert.Equal(ErrorType.INVALID_USERNAME, actual.ErrorType);
    }
}
