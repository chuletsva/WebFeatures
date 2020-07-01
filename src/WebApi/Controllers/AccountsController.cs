using Application.Features.Accounts.Login;
using Application.Features.Accounts.Register;
using Application.Infrastructure.Results;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    /// <summary>
    /// Работа с аккаунтами пользователей
    /// </summary>
    public class AccountsController : BaseController
    {
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

            await SignInUser((user.Id, user.Roles));

            return Ok();
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

            await SignInUser((user.Id, user.Roles));

            return Ok();
        }

        private async Task SignInUser((Guid Id, string[] Roles) user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // TODO: token
        }
    }
}