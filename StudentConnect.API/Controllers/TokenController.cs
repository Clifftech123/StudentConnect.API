using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentConnect.API.Models.Domain;
using StudentConnect.API.Models.DTO;
using StudentConnect.API.Repositories.Abstract;

namespace StudentConnect.API.Controllers
{
  
    [ApiController]
    [Route("api/account/v1")]
    public class TokenController : ControllerBase
    { 
        private readonly  DatabaseContext databaseContext;
        private readonly IAuthenticationService tokenService;

        public TokenController(DatabaseContext databaseContext, IAuthenticationService tokenService)
        {
            this.databaseContext = databaseContext;
            this.tokenService = tokenService;
        }

        // Refresh Token
        [HttpGet("refresh-token")]
        public IActionResult Refresh (RefreshTokenRequest tokenRequest)
        {
            if (tokenRequest is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenRequest.AccessToken;
            string refreshToken = tokenRequest.RefreshToken;
            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = databaseContext.TokenInfo.SingleOrDefault(u => u.UserName == username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= System.DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = tokenService.GetToken(principal.Claims);
            var newRefreshToken = tokenService.GetRefreshToken();
            user.RefreshToken = newRefreshToken;
            databaseContext.SaveChanges();

            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        // Revoke Token


        // : TO ADDING OF  Authorize


        [HttpPost("removke")]
        public IActionResult Revoke()
        {
            try
            {
                var username = User.Identity.Name;
                var user = databaseContext.TokenInfo.SingleOrDefault(u => u.UserName == username);
                if (user == null) return BadRequest();
                user.RefreshToken = null;
                databaseContext.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
