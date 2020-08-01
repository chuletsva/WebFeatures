using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.CommonServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Settings;

namespace WebApi.Authentication
{
    public class TokenProvider : ITokenProvider
    {
        private readonly JwtSettings _settings;
        private readonly IDateTime _dateTime;

        public TokenProvider(IOptions<JwtSettings> settings, IDateTime dateTime)
        {
            _settings = settings.Value;
            _dateTime = dateTime;
        }

        public string CreateToken(ClaimsIdentity identity)
        {
            byte[] key = Encoding.UTF8.GetBytes(_settings.Key);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                Expires = _dateTime.Now.AddMinutes(_settings.Lifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();

            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}