using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_Securite.Utils
{
    public class EncryptionManager
    {
        private string _Pepper { get; set; }

        public EncryptionManager(IConfiguration config)
        {
            _Pepper = config.GetValue<string>("EncryptionPepper");
        }

        private string PreparePassword(string password, string salt)
        {
            return password + salt + _Pepper;
        }

        public string Hash(string password, string salt)
        {
            string pwd = PreparePassword(password, salt);
            return BCrypt.Net.BCrypt.EnhancedHashPassword(pwd, BCrypt.Net.HashType.SHA512);
        }

        public bool Verify(string password, string salt, string passwordHash)
        {
            string pwd = PreparePassword(password, salt);
            return BCrypt.Net.BCrypt.EnhancedVerify(pwd, passwordHash, BCrypt.Net.HashType.SHA512);
        }

        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
    }
}
