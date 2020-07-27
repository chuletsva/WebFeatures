using Domian.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Tests.Common.Stubs
{
    public class CustomSoftDeleteEntity : Entity, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
