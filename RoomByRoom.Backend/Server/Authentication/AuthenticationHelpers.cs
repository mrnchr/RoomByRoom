using System.Security.Cryptography;

namespace Server.Authentication
{
    public static class AuthenticationHelpers
    {
        public static void ProvideSaltAndHash(this EncryptedData obj, string password)
        {
            byte[] salt = GenerateSalt();

            obj.Salt = Convert.ToBase64String(salt);
            obj.Password = ComputeHash(password, obj.Salt);
        }

        private static byte[] GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var salt = new byte[24];
            rng.GetBytes(salt);
            return salt;
        }

        public static string ComputeHash(string password, string saltString)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            
            using var hashGenerator = new Rfc2898DeriveBytes(password, salt);
            hashGenerator.IterationCount = 10101;
            byte[] bytes = hashGenerator.GetBytes(24);
            return Convert.ToBase64String(bytes);
        }
    }
}