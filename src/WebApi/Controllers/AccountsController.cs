using Application.Features.Accounts.Login;
using Application.Features.Accounts.Register;
using Application.Infrastructure.Results;
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
using WebApi.Settings;

namespace WebApi.Controllers
{
    /// <summary>
    /// Работа с аккаунтами пользователей
    /// </summary>
    public class AccountsController : BaseController
    {
        private readonly IDateTime _dateTime;
        private readonly JwtSettings _jwtSettings;

        public AccountsController(IDateTime dateTime, IOptions<JwtSettings> jwtSettings)
        {
            _dateTime = dateTime;
            _jwtSettings = jwtSettings.Value;
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

            string token = CreateToken((user.Id, user.Roles));

            return Ok(token);
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

            string token = CreateToken((user.Id, user.Roles));

            return Ok(token);
        }

        private string CreateToken((Guid Id, string[] Roles) user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = _dateTime.Now.AddMinutes(_jwtSettings.Lifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();

            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}