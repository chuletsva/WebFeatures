using Application.Interfaces.Security;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string ComputeHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(password));
            }

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