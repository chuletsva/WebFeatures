using System;

namespace Application.Common.Interfaces.CommonServices
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        bool IsAuthenticated { get; }
    }
}
