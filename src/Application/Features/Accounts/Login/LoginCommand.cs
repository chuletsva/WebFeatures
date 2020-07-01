using Application.Infrastructure.Requests;

namespace Application.Features.Accounts.Login
{
    /// <summary>
    /// Войти в систему
    /// </summary>
    public class LoginCommand : CommandBase<UserLoginDto>
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