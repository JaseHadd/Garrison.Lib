using System.Security.Cryptography;
using Garrison.Lib.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Garrison.Lib;

public class PasswordHasher()
{
    private readonly record struct Config(KeyDerivationPrf Prf, int Iterations, int SubKeyLength, int SaltLength);

    private static Config s_v0 = new Config(KeyDerivationPrf.HMACSHA512, 10_000, 256 / 8, 128 / 8);

    private static readonly Dictionary<byte, Config> s_configs = new Dictionary<byte, Config> { [0x00] = s_v0 };

    public byte[] HashPassword(string password)
    {
        Config config = s_v0;

        byte[] salt = new byte[config.SaltLength];
        byte[] subKey;

        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(salt);
        subKey = KeyDerivation.Pbkdf2(password, salt, config.Prf, config.Iterations, config.SubKeyLength);

        return [
            0x00,
            ..salt,
            ..subKey
        ];
    }

    public bool VerifyPassword(User user, string password)
    {
        var bytes = user.Password;
        
        if (bytes.Length == 0)
            return false;

        var config = s_configs[bytes[0]];
        var salt = new byte[config.SaltLength];
        var subKey = new byte[config.SubKeyLength];

        byte[] newKey;

        Buffer.BlockCopy(bytes, 1, salt, 0, salt.Length);
        Buffer.BlockCopy(bytes, 1 + salt.Length, subKey, 0, subKey.Length);

        newKey = KeyDerivation.Pbkdf2(password, salt, config.Prf, config.Iterations, config.SubKeyLength);

        return subKey.SequenceEqual(newKey);
    }
}