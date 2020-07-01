using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using VidyaSadhan_API.Entities;

namespace VidyaSadhan_API.Helpers
{
    public class Utilities : IPasswordHasher<UserCt>
    {

        public string HashPassword(UserCt user, string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(UserCt user, string hashedPassword, string providedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
