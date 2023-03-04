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

    public string Hash(string data)
    {
        return Convert.ToBase64String(HashPassword(data));
    }

    public bool VerifyHash(string data, string hash)
    {
        byte[] hashByte = HashPassword(data);
        return Convert.FromBase64String(hash).SequenceEqual(hashByte);
    }

    public async Task<string> HashAsync(string data)
    {
        byte[] hashByte = await HashPasswordAsync(data);
        return Convert.ToBase64String(hashByte);
    }

    public async Task<bool> VerifyHashAsync(string data, string hash)
    {
        byte[] hashByte = await HashPasswordAsync(data);
        return Convert.FromBase64String(hash).SequenceEqual(hashByte);
    }

    private byte[] HashPassword(string password)
    {
        var argon2Hash = SetupArgon2(password);
        return argon2Hash.GetBytes(_settings.TagLength);
    }

    private async Task<byte[]> HashPasswordAsync(string password)
    {
        var argon2Hash = SetupArgon2(password);
        return await argon2Hash.GetBytesAsync(_settings.TagLength);
    }

    private Argon2id SetupArgon2(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        using var argon2 = new Argon2id(bytes);

        argon2.DegreeOfParallelism = _settings.DegreeOfParallelism;
        argon2.Iterations = _settings.Iterations;
        argon2.MemorySize = _settings.MemorySize;

        return argon2;
    }
}
