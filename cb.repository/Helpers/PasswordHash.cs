//dotnet add package CryptoHelper --version 3.0.2
using CryptoHelper;
namespace CP.Api.Helpers
{
  public sealed class PasswordHash
  {
    // Hash a password
    public static string HashPassword(string password)
    {
      return Crypto.HashPassword(password);
    }

    // Verify the password hash against the given password
    public static bool VerifyPassword(string hash, string password)
    {
      return Crypto.VerifyHashedPassword(hash, password);
    }
  }
}