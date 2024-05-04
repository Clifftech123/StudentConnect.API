using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentConnect.API.src.Interface;
using StudentConnect.API.src.Models.Dto;

namespace StudentConnect.API.src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (registerUserDto == null)
            {
                return BadRequest("The registration data cannot be null");
            }

            try
            {
                var registrationResponse = await userService.RegisterAsync(registerUserDto);
                return StatusCode(201, new { message = "User registered successfully", data = registrationResponse });
            }
            catch (Exception ex)
            {
                // Log the exception and return a 400 Bad Request response with the error message
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var loginResponse = await userService.LoginAsync(loginUserDto);
            return Ok(new { message = "User logged in successfully", data = loginResponse });
        }

        //[Authorize]
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> Get()
        {
            var users = await userService.GetAllAsync();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);

        }



        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                await userService.DeleteUserAsync(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }







    }
}
