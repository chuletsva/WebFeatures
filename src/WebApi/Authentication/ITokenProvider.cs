using System.Security.Claims;

namespace WebApi.Authentication
{
    public interface ITokenProvider
    {
        string CreateToken(ClaimsIdentity identity);
    }
}