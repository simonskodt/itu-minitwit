using MongoDB.Driver;

namespace MiniTwit.Tests.Infrastructure.Repositories;

public class FollowerRepositoryTests : RepoTests
{
    private readonly FollowerRepository _repository;

    public FollowerRepositoryTests()
    {
        _repository = new FollowerRepository(_context);
    }

    [Fact]
    public void Create_given_existing_target_username_creates_new_follower_and_returns_Created()
    {
        var follower = new Follower
        {
            WhoId = "000000000000000000000001",
            WhomId = "000000000000000000000004"
        };

        var expected = new Response<Follower>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = follower
        };

        var actual = _repository.Create("000000000000000000000001", "Victor");

        Assert.Equal(HTTPResponse.Created, actual.HTTPResponse);
        Assert.Equal("000000000000000000000001", actual.Model!.WhoId);
        Assert.Equal("000000000000000000000004", actual.Model!.WhomId);
    }

    [Fact]
    public void Create_given_non_existing_target_username_returns_NotFound()
    {
        var actual = _repository.Create("000000000000000000000001", "test");

        Assert.Equal(HTTPResponse.NotFound, actual.HTTPResponse);
        Assert.Null(actual.Model);
    }

    [Fact]
    public void Delete_given_existing_targetUsername_deletes_user_and_returns_NoContent()
    {
        var existing = _context.Followers.Find(f => f.WhoId == "000000000000000000000001" && f.WhomId == "000000000000000000000002").FirstOrDefault();

        Assert.NotNull(existing);
        Assert.Equal("000000000000000000000001", existing.WhoId);
        Assert.Equal("000000000000000000000002", existing.WhomId);

        var actual = _repository.Delete("000000000000000000000001", "Simon");

        Assert.Equal(HTTPResponse.NoContent, actual.HTTPResponse);
        
        existing = _context.Followers.Find(f => f.WhoId == "000000000000000000000001" && f.WhomId == "000000000000000000000002").FirstOrDefault();

        Assert.Null(existing);
    }

    [Fact]
    public void Delete_given_non_existing_targetUsername_returns_NotFound()
    {
        var actual = _repository.Delete("000000000000000000000001", "test");

        Assert.Equal(HTTPResponse.NotFound, actual.HTTPResponse);
    }
}
