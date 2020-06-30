using System;

namespace Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}
