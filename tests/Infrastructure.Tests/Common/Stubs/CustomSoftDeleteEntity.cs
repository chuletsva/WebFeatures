using Domian.Common;

namespace Infrastructure.Tests.Common.Stubs
{
    public class CustomSoftDeleteEntity : Entity, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
