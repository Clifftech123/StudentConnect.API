using StudentConnect.API.Models.DTO;
using System.Security.Claims;

namespace StudentConnect.API.Repositories.Abstract
{
    public interface IAuthenticationService
    {
        TokenResponse GetToken(IEnumerable<Claim> claim);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
