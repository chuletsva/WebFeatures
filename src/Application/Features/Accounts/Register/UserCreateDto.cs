using System;

namespace Application.Features.Accounts.Register
{
    /// <summary>
    /// Созданный пользователь
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public string[] Roles { get; set; }

        public static implicit operator (Guid Id, string[] Roles)(UserCreateDto dto)
        {
            return (dto.Id, dto.Roles);
        }
    }
}
