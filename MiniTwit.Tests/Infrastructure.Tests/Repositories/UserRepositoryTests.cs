using MiniTwit.Security;
using Moq;

namespace MiniTwit.Tests.Infrastructure.Repositories;

public class UserRepositoryTests : RepoTests
{
    private readonly UserRepository _repository;
    private Mock<IHasher> _hasher;

    public UserRepositoryTests()
    {
        _hasher = new Mock<IHasher>();
        var password = "password";
        _hasher.Setup(h => h.Hash("password", out password));
        _repository = new UserRepository(_context, _hasher.Object);
    }

    [Fact]
    public void Create_given_unused_username_creates_new_user_and_returns_Created()
    {
        var user = new User
        {
            Username = "test",
            Email = "t@test.com",
            Password = "password"
        };

        var expected = new Response<User>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = user
        };

        var actual = _repository.Create(user.Username, user.Email, user.Password);

        Assert.Equal(HTTPResponse.Created, actual.HTTPResponse);
        Assert.Equal(expected.Model.Username, actual.Model!.Username);
        Assert.Equal(expected.Model.Email, actual.Model!.Email);
        Assert.Equal(expected.Model.Password, actual.Model!.Password);
    }

    [Fact]
    public void Create_given_used_username_returns_Conflict()
    {
        var expected = new Response<User>
        {
            HTTPResponse = HTTPResponse.Conflict,
            Model = null
        };

        var actual = _repository.Create("Gustav", "t@test.com", "password");

        Assert.Equal(HTTPResponse.Conflict, actual.HTTPResponse);
        Assert.Null(actual.Model);
    }
    [Fact]
    public void GetByUserId_given_existing_id_returns_user()
    {
        var actual = _repository.GetByUserId("000000000000000000000001");

        Assert.Equal(HTTPResponse.Success, actual.HTTPResponse);
        Assert.Equal("000000000000000000000001", actual.Model!.Id);
    }

    [Fact]
    public void GetByUserId_given_non_existing_id_returns_NotFound()
    {
        var actual = _repository.GetByUserId("000000000000000000000000");

        Assert.Equal(HTTPResponse.NotFound, actual.HTTPResponse);
        Assert.Null(actual.Model);
    }

    [Fact]
    public void GetByUsername_given_existing_username_returns_user()
    {
        var actual = _repository.GetByUsername("Gustav");

        Assert.Equal(HTTPResponse.Success, actual.HTTPResponse);
        Assert.Equal("Gustav", actual.Model!.Username);
    }

    [Fact]
    public void GetByUsername_given_non_existing_username_returns_NotFound()
    {
        var actual = _repository.GetByUsername("test");

        Assert.Equal(HTTPResponse.NotFound, actual.HTTPResponse);
        Assert.Null(actual.Model);
    }
}
