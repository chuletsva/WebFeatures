using Domian.Common;
using System.Collections.Generic;

namespace Domian.Entities.Accounts
{
    public class Role : Entity
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
            Permissions = new HashSet<RolePermission>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<UserRole> UserRoles { get; private set; }

        public ICollection<RolePermission> Permissions { get; private set; }
    }
}
