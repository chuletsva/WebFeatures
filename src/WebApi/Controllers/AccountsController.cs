using Application.Features.Accounts.Login;
using Application.Features.Accounts.Register;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Models.Dto;
using Application.Common.Models.Results;
using WebApi.Authentication;
using WebApi.Controllers.Base;

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
            UserInfoDto user = await Mediator.Send(request);

            return Ok(LoginUser(user));
        }

        /// <summary>
        /// Войти на сайт
        /// </summary>
        /// <response code="200">Успех</response>
        /// <response code="400" cref="ValidationError">Неверные пользовательские данные</response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([Required] LoginCommand request)
        {
            UserInfoDto user = await Mediator.Send(request);

            return Ok(LoginUser(user));
        }

        private object LoginUser(UserInfoDto user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

            string token = _tokenProvider.CreateToken(identity);

            return new { id = user.Id, access_token = token };
        }
    }
}