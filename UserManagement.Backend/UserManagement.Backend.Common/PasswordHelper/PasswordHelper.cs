using System.Security.Cryptography;

namespace UserManagement.Backend.Common.PasswordHelper;

public static class PasswordHelper
{
    public static string HashPassword(string password, byte[] salt)
    {
        using Rfc2898DeriveBytes hashThingy = new(password, salt, 10_000, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(hashThingy.GetBytes(32));
    }

    public static byte[] GenerateSalt()
    {
        var salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    public static bool ValidLoginPassword(string password, string salt, string hash)
    {
        return HashPassword(password, Convert.FromBase64String(salt)) == hash;
    }

    public static string GenerateStrongPassword()
    {
        string charSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        int passwordLength = 8;
        char[] password = new char[passwordLength];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[passwordLength];
            rng.GetBytes(randomBytes);

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = charSet[randomBytes[i] % charSet.Length];
            }
        }
        return new string(password);
    }
}