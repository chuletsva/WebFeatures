using System;

namespace Application.Features.Accounts.Login
{
    /// <summary>
    /// Залогиненый пользователь
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public string[] Roles { get; set; }

        public static implicit operator (Guid Id, string[] Roles)(UserLoginDto dto)
        {
            return (dto.Id, dto.Roles);
        }
    }
}
