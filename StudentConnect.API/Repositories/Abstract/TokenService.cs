using StudentConnect.API.Models.DTO;
using System.Security.Claims;

namespace StudentConnect.API.Repositories.Abstract
{
    public interface ITokenService
    {
        TokenResponse GetToken(IEnumerable<Claim> claim);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
