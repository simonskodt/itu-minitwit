namespace MiniTwit.Security;

public interface IHasher
{
    string Hash(string data);
    bool VerifyHash(string data, string hash);
    Task<bool> VerifyHashAsync(string data, string hash);
    Task<string> HashAsync(string data);
}