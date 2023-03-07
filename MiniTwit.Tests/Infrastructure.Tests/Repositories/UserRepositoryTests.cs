using MiniTwit.Core.Responses;
using MiniTwit.Core.Error;
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
        _hasher.Setup(h => h.Hash("password"));
        _repository = new UserRepository(_context);
    }

    [Fact]
    public void Create_given_unused_username_creates_new_user_and_returns_create_user_without_error()
    {
        var user = new User
        {
            Username = "test",
            Email = "t@test.com",
            Password = "password"
        };

        var expected = new DBResult<User>
        {
            Model = user,
            ErrorType = null
        };

        var actual = _repository.Create(user.Username, user.Email, user.Password);

        Assert.Equal(expected.Model.Username, actual.Model!.Username);
        Assert.Equal(expected.Model.Email, actual.Model!.Email);
        Assert.Equal(expected.Model.Password, actual.Model!.Password);
    }

    [Fact]
    public void GetByUserId_given_existing_id_returns_user()
    {
        var actual = _repository.GetByUserId("000000000000000000000001");

        Assert.Equal(null, actual.ErrorType);
        Assert.Equal("000000000000000000000001", actual.Model!.Id);
    }

    [Fact]
    public void GetByUserId_given_non_existing_id_returns_ErrorType_INVALID_USER_ID()
    {
        var actual = _repository.GetByUserId("000000000000000000000000");

        Assert.Equal(ErrorType.INVALID_USER_ID, actual.ErrorType);
        Assert.Null(actual.Model);
    }

    [Fact]
    public void GetByUsername_given_existing_username_returns_user()
    {
        var actual = _repository.GetByUsername("Gustav");

        Assert.Equal(null, actual.ErrorType);
        Assert.Equal("Gustav", actual.Model!.Username);
    }

    [Fact]
    public void GetByUsername_given_non_existing_username_returns_INVALID_USERNAME()
    {
        var actual = _repository.GetByUsername("test");

        Assert.Equal(ErrorType.INVALID_USERNAME, actual.ErrorType);
        Assert.Null(2);
    }
}
