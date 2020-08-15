using System;

namespace Application.Common.Models.Dto
{
    public class UserInfoDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public string[] Roles { get; set; }
    }
}
