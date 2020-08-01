using System;
using System.Linq;
using System.Security.Claims;
using Application.Interfaces.CommonServices;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.CommonServices
{
    public class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            HttpContext context = contextAccessor.HttpContext;

            if (context?.User == null) return;
            
            Claim idClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (idClaim == null || !Guid.TryParse(idClaim.Value, out Guid id)) return;
            
            UserId = new Guid(idClaim.Value);
            
            IsAuthenticated = true;
        }

        public Guid UserId { get; }
        public bool IsAuthenticated { get; }
    }
}