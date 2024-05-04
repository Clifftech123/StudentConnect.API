using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentConnect.API.src.Data;
using StudentConnect.API.src.Infrastructure.Enums;
using StudentConnect.API.src.Models.Data;

namespace StudentConnect.API.src.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;
        private readonly DbSet<User> _users;
        private readonly UserManager<User> _userManager;

        public UserRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _users = _context.Set<User>();
            _userManager = userManager;
        }

        public async Task DeleteUserAsync(string userId)
        {
            var userToDelete = await _users.FirstOrDefaultAsync(u => u.Id == userId);

            if (userToDelete == null)
            {
                throw new Exception("User not found");
            }

            _users.Remove(userToDelete);

            await SaveChangesAsync();
        }


        // Get all user from the database 
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users.ToListAsync();
        }



         // Get find user by email
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.FirstOrDefaultAsync(x => x.Email == email);
        }


        // Find  user by username 
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _users.FirstOrDefaultAsync(x => x.UserName == username);
        }


        // checking oif the password is correct 
        public async Task<bool> IsPasswordCorrectAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }


         // Register user 
        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
           return await _userManager.CreateAsync(user, password);
        }


        // Saving the changes made  by the user into the database 
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error coming from how your saving everything here ");
            }
        }



        // User status in  the system
        public async Task SetStatusesAsync(List<string> userIds, Status status)
        {
            var usersToUpdate = await _users.Where(u => userIds.Contains(u.Id)).ToListAsync();

            foreach (var user in usersToUpdate)
            {
                user.Status = status;
            }

            await SaveChangesAsync();
        }

       
    }
}
