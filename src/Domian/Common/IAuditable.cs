using System;

namespace Domian.Common
{
    public interface IAuditable
    {
        DateTime CreateDate { get; set; }
        Guid CreatedById { get; set; }

        DateTime UpdateDate { get; set; }
        Guid UpdatedById { get; set; }
    }
}
