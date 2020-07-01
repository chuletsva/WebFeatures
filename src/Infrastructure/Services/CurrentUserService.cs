using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Infrastructure.Services
{
    class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            HttpContext context = contextAccessor.HttpContext;

            if (context?.User != null)
            {
                Claim idClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (idClaim != null && Guid.TryParse(idClaim.Value, out Guid id))
                {
                    UserId = new Guid(idClaim.Value);
                }
            }
        }

        public Guid UserId { get; }
    }
}