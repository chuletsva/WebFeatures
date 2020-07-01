using Application.Interfaces.Security;
using Microsoft.AspNetCore.DataProtection;
using System;

namespace Infrastructure.Security
{
    class PasswordHasher : IPasswordHasher
    {
        private readonly IDataProtector _dataProtector;

        public PasswordHasher(IDataProtectionProvider protectionProvider)
        {
            _dataProtector = protectionProvider.CreateProtector("Hashing passwords");
        }

        public string ComputeHash(string passord)
        {
            return passord;
        }

        public bool Verify(string hash, string expectedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
