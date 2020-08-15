using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;
using Application.Common.Interfaces.Security;

namespace Infrastructure.Security
{
    internal class PasswordHasher : IPasswordHasher
    {
        public string ComputeHash(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes("1ec5cf41-a882-4fc8-806d-0d90ddfd488d"),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(hash);
        }

        public bool Verify(string hash, string expectedPassword)
        {
            return hash == ComputeHash(expectedPassword);
        }
    }
}