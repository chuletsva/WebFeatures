using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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

                Roles = context.User.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value)
                    .ToHashSet();

                IsAuthenticated = true;
            }
        }

        public Guid UserId { get; }
        public ICollection<string> Roles { get; }
        public bool IsAuthenticated { get; }
    }
}