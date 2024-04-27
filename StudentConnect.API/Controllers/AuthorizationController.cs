using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentConnect.API.Models;
using StudentConnect.API.Models.Domain;
using StudentConnect.API.Models.DTO;
using StudentConnect.API.Repositories.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudentConnect.API.Controllers
{

    [Route("api/[controller]/{action}")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenService _tokenService;


        private const int StatusCodeInvalid = 0;
      

        public AuthorizationController(DatabaseContext databaseContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole>
            roleManager, ITokenService tokenService
            )
        {
            this.databaseContext = databaseContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _tokenService = tokenService;
        }



        // Register User
        private const int StatusCodeSuccess = 1;
        private const string MessageInvalidFields = "Please pass all the required fields";
        private const string MessageUserExists = "User already exists!";
        private const string MessageUserCreationFailed = "User creation failed! Please check user details and try again.";
        private const string MessageRegistrationSuccess = "Successfully registered";

        public async Task<IActionResult> Registration([FromBody] RegistrationModel model)
        {
            var status = await RegisterUser(model, UserRoles.User);
            return status.StatusCode == StatusCodeSuccess ? Ok(status) : BadRequest(status);
        }

        // Register Admin
        [HttpPost]
        public async Task<IActionResult> RegistrationAdmin([FromBody] RegistrationModel model)
        {
            var status = await RegisterUser(model, UserRoles.Admin);
            return Ok(status);
        }


        // Register New User
        private async Task<Status> RegisterUser(RegistrationModel model, string role)
        {
            var status = new Status();

            if (!ModelState.IsValid)
            {
                status.StatusCode = StatusCodeInvalid;
                status.Message = MessageInvalidFields;
                return status;
            }

            // Check if the user already exists
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = StatusCodeInvalid;
                status.Message = MessageUserExists;
                return status;
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = model.Email,
                Name = model.Name
            };

            // Create user 
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = StatusCodeInvalid;
                status.Message = MessageUserCreationFailed;
                return status;
            }

            // Add user to role
            await userManager.AddToRoleAsync(user, role);

            status.StatusCode = StatusCodeSuccess;
            status.Message = MessageRegistrationSuccess;
            return status;
        }



        // Login User

        private const string MessageInvalidCredentials = "Invalid Username or Password";
        private const string MessageLoginSuccess = "Logged in";
        private const string MessageServerError = "An error occurred while processing your request";

        // Login User
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                return Ok(new LoginResponse
                {
                    StatusCode = StatusCodeInvalid,
                    Message = MessageInvalidCredentials,
                    Token = "",
                    Expiration = null
                });
            }

            var userRole = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate Token for user
            var token = _tokenService.GetToken(claims);
            var refreshToken = _tokenService.GetRefreshToken();
            var tokenInfo = databaseContext.TokenInfo.SingleOrDefault(u => u.UserName == user.UserName);
            if (tokenInfo == null)
            {
                tokenInfo = new TokenInfo
                {
                    UserName = user.UserName,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
                };
                databaseContext.TokenInfo.Add(tokenInfo);
            }
            else
            {
                tokenInfo.RefreshToken = refreshToken;
                tokenInfo.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            }

            try
            {
                databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception here
                return BadRequest(MessageServerError);
            }

            return Ok(new LoginResponse
            {
                Name = user.Name,
                Username = user.UserName,
                Token = token.TokenString,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
                StatusCode = StatusCodeSuccess,
                Message = MessageLoginSuccess
            });
        }




        // Check password


        private const string MessageUserNotFound = "User not found";
        private const string MessageInvalidCurrentPassword = "Invalid current password";
        private const string MessagePasswordChangeFailed = "Password change failed";
        private const string MessagePasswordChangedSuccessfully = "Password changed successfully";

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = StatusCodeInvalid;
                status.Message = MessageInvalidFields;
                return BadRequest(status);
            }

            // find user
            var user = await userManager.FindByNameAsync(model.Username);
            if (user is null)
            {
                status.StatusCode = StatusCodeInvalid;
                status.Message = MessageUserNotFound;
                return BadRequest(status);
            }

            // check current password
            if (!await userManager.CheckPasswordAsync(user, model.CurrentPassword))
            {
                status.StatusCode = StatusCodeInvalid;
                status.Message = MessageInvalidCurrentPassword;
                return BadRequest(status);
            }

            // change password
            try
            {
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    status.StatusCode = StatusCodeInvalid;
                    status.Message = MessagePasswordChangeFailed;
                    return BadRequest(status);
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                return BadRequest(MessageServerError);
            }

            status.StatusCode = StatusCodeSuccess;
            status.Message = MessagePasswordChangedSuccessfully;
            return Ok(status);
        }


    }
}

