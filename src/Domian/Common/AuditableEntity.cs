using Domian.Entities.Accounts;
using System;

namespace Domian.Common
{
    public class AuditableEntity : Entity
    {
        public DateTime CreatedAt { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedById { get; set; }
        public User UpdatedBy { get; set; }
    }
}
