using System;

namespace Application.Interfaces.CommonServices
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        bool IsAuthenticated { get; }
    }
}
