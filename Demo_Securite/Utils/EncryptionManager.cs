using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_Securite.Utils
{
    public static class EncryptionManager
    {
        private static string PreparePassword(string password, string salt)
        {
            return password + salt;
        }

        public static string Hash(string password, string salt)
        {
            string pwd = PreparePassword(password, salt);
            return BCrypt.Net.BCrypt.HashPassword(pwd);
        }

        public static bool Verify(string password, string salt, string passwordHash)
        {
            string pwd = PreparePassword(password, salt);
            return BCrypt.Net.BCrypt.Verify(pwd, passwordHash);
        }

        public static string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
    }
}
