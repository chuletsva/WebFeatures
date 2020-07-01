using System;

namespace Application.Interfaces.Services
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
    }
}
