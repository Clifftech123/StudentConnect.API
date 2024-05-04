using StudentConnect.API.src.Infrastructure.Enums;
using StudentConnect.API.src.Models.Dto;

namespace StudentConnect.API.src.Interface
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetAllAsync();

        Task<RegistrationResponseDto> RegisterAsync(RegisterUserDto registerUserDto);

        Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto);

        public Task SetStatusesAsync(List<string> userIds, Status status);

        public Task DeleteUserAsync(string userId);


        public Task<bool> IsUserBanned(string username);
    }
}
