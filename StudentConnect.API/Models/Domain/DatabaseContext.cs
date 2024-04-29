using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentConnect.API.Models.Domain
{
    public class DatabaseContext :IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        /// <summary>
        ///  Adding refresh  to the Database
        /// </summary>
        public DbSet<TokenInfo> TokenInfo { get; set; }

    }
}
