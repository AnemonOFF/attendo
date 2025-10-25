using System.Security.Cryptography;
using System.Text;

namespace Attendo.Infrastructure.Security;

public class Sha256PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);

        using var sha = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha.ComputeHash(combined);
        var hashBase64 = Convert.ToBase64String(hash);

        return $"SHA256${salt}${hashBase64}";
    }

    public bool Verify(string password, string storedHash)
    {
        try
        {
            var parts = storedHash.Split('$');
            if (parts.Length != 3 || parts[0] != "SHA256") return false;

            var salt = parts[1];
            var expectedHash = parts[2];

            using var sha = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var computed = Convert.ToBase64String(sha.ComputeHash(combined));

            return computed == expectedHash;
        }
        catch
        {
            return false;
        }
    }
}
