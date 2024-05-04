using StudentConnect.API.src.Exceptions;
using StudentConnect.API.src.Infrastructure.Enums;
using StudentConnect.API.src.Interface;
using StudentConnect.API.src.Models.Data;
using StudentConnect.API.src.Models.Dto;
using StudentConnect.API.src.Repositories;

namespace StudentConnect.API.src.Services
{
    public class UserService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator) : IUserService

    {

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                guid = Guid.Parse(u.Id),
                Name = u.UserName,
                Email = u.Email,
                LastLogin = u.LastLogin,
                Status = u.Status
            });
        }



        public async Task<RegistrationResponseDto> RegisterAsync(RegisterUserDto registerUserDto)
        {
            // Check if a user with the same email already exists
            var existingUser = await userRepository.GetByEmailAsync(registerUserDto.Email);
            if (existingUser != null)
            {
                throw new RegisterException("A user with this email already exists.");
            }

            // Continue with the registration process if no such user exists
            User user = new()
            {
                Name = registerUserDto.Name,
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                School = registerUserDto.School,
                Twon = registerUserDto.Twon,
                Program = registerUserDto.Program,

            };


            var result = await userRepository.RegisterAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new RegisterException(errors);
            }

            return new RegistrationResponseDto
            {
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                school = user.School,
                Twon = user.Twon,
                Program = user.Program,

            };
        }





        public async Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await userRepository.GetByEmailAsync(loginUserDto.Email);

            // Check if user exists before trying to use the user object
            if (user == null)
            {
                throw new LoginException("User does not exist");
            }

            var isCorrect = await userRepository.IsPasswordCorrectAsync(user, loginUserDto.Password);
            if (!isCorrect)
            {
                throw new LoginException("Username or password is incorrect");
            }

            // Check if user is banned before updating LastLogin
            if (await IsUserBanned(user.UserName))
            {
                throw new UserBannedException("You are banned from accessing this resource.");
            }

            user.LastLogin = DateTime.Now;
            await userRepository.SaveChangesAsync();

            var token = jwtTokenGenerator.GenerateToken(user);

            return new LoginResponseDto
            {
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
            };
        }




        public async Task SetStatusesAsync(List<string> userIds, Status status)
        {
            await userRepository.SetStatusesAsync(userIds, status);
        }



        // delete user 
        public async Task DeleteUserAsync(string userId)
        {
            await userRepository.DeleteUserAsync(userId);
        }



        public async Task<bool> IsUserBanned(string username)
        {
            var user = await userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                throw new UserDeletedException("Your account had been deleted. Cannot access the resource");
            }
            return user.Status == Status.Blocked;
        }






    }
}
