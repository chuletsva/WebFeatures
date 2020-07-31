using Application.Features.Accounts.Login;
using Application.Features.Accounts.Register;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Results;
using WebApi.Controllers.Base;
using WebApi.Settings;

namespace WebApi.Controllers
{
    /// <summary>
    /// Работа с аккаунтами пользователей
    /// </summary>
    public class AccountsController : BaseController
    {
        private readonly ITokenProvider _tokenProvider;

        public AccountsController(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Зарегистрировать нового пользователя
        /// </summary>
        /// <response code="200">Успех</response>
        /// <response code="400" cref="ValidationError">Неверные пользовательские данные</response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([Required] RegisterCommand request)
        {
            UserCreateDto user = await Mediator.Send(request);

            ClaimsIdentity identity = GetUserIdentity(user);

            string token = _tokenProvider.CreateToken(identity);

            return Ok(new { user.Id, token });
        }

        /// <summary>
        /// Войти на сайт
        /// </summary>
        /// <response code="200">Успех</response>
        /// <response code="400" cref="ValidationError">Неверные пользовательские данные</response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody, Required] LoginCommand request)
        {
            UserLoginDto user = await Mediator.Send(request);

            ClaimsIdentity identity = GetUserIdentity(user);

            string token = _tokenProvider.CreateToken(identity);

            return Ok(new { user.Id, token });
        }

        private ClaimsIdentity GetUserIdentity((Guid Id, string[] Roles) user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x)));

            return new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        }
    }

    public interface ITokenProvider
    {
        string CreateToken(ClaimsIdentity indentity);
    }

    public class TokenProvider
    {
        private readonly JwtSettings _settings;
        private readonly IDateTime _dateTime;

        public TokenProvider(IOptions<JwtSettings> settings, IDateTime dateTime)
        {
            _settings = settings.Value;
            _dateTime = dateTime;
        }

        public string CreateToken(ClaimsIdentity indentity)
        {
            byte[] key = Encoding.UTF8.GetBytes(_settings.Key);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = indentity,
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