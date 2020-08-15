using Domian.Common;
using System;
using System.Collections.Generic;

namespace Domian.Entities.Accounts
{
    public class User : Entity
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public Guid? PictureId { get; set; }
        public File Picture { get; set; }

        public ICollection<UserRole> UserRoles { get; private set; }
    }
}
