using System;
using System.Collections.Generic;

namespace Application.Interfaces.Services
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        ICollection<string> Roles { get; }

        bool IsAuthenticated { get; }
    }
}
