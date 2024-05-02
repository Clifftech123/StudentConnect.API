using StudentConnect.API.src.Infrastructure.Enums;
using StudentConnect.API.src.Models.Dto;

namespace StudentConnect.API.src.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetAllAsync();

        public Task RegisterAsync(RegisterUserDto registerUserDto);

        public Task<string> LoginAsync(LoginUserDto loginUserDto);

        public Task SetStatusesAsync(List<string> userIds, Status status);

        public Task DeleteUsersAsync(List<string> userIds);

        public Task<bool> IsUserBanned(string username);
    }
}
