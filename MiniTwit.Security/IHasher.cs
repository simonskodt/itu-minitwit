namespace MiniTwit.Security;

public interface IHasher
{
    void Hash(string data, out string hash);
    bool VerifyHash(string data, string hash);
}