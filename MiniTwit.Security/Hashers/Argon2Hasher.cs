using Microsoft.Extensions.Options;
using System.Text;
using Konscious.Security.Cryptography;

namespace MiniTwit.Security.Hashers;

public class Argon2Hasher : IHasher
{
    private readonly HashSettings _settings;

    public Argon2Hasher(IOptions<HashSettings> settings)
    {
        _settings = settings.Value;
    }

    public void Hash(string data, out string hash)
    {
        hash = Convert.ToBase64String(Hash(data));
    }

    private byte[] Hash(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        using var argon2 = new Argon2id(bytes);

        argon2.DegreeOfParallelism = _settings.DegreeOfParallelism;
        argon2.Iterations = _settings.Iterations;
        argon2.MemorySize = _settings.MemorySize * 128;
        return argon2.GetBytes(32);
    }

    public bool VerifyHash(string data, string hash)
    {
        byte[] hashByte = Hash(data);
        return Convert.FromBase64String(hash).SequenceEqual(hashByte);
    }
}
