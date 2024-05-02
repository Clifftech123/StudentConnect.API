using Microsoft.AspNetCore.Identity;
using StudentConnect.API.src.Infrastructure.Enums;
using StudentConnect.API.src.Models.Data;

namespace StudentConnect.API.src.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync();

        public Task<User?> GetByEmailAsync(string email);

        public Task<User?> GetByUsernameAsync(string username);

        public Task<bool> IsPasswordCorrectAsync(User user, string password);

        public Task<IdentityResult> RegisterAsync(User user, string password);

        public Task SetStatusesAsync(List<string> userIds, Status status);

        public Task DeleteUsersAsync(List<string> userIds);

        public Task SaveChangesAsync();
    }
}
