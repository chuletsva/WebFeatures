using Application.Common.Models.Dto;
using Application.Common.Models.Requests;

namespace Application.Features.Accounts.Login
{
    /// <summary>
    /// Войти в систему
    /// </summary>
    public class LoginCommand : CommandBase<UserInfoDto>
    {
        /// <summary>
        /// E-mail пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}