using Microsoft.AspNetCore.Identity;
using StudentConnect.API.src.Infrastructure.Enums;

namespace StudentConnect.API.src.Models.Data
{
    public class User : IdentityUser
    {
        public DateTime LastLogin { get; set; }

        public Status Status { get; set; }
        public string Name { get; set; }
        public string School { get; set; }

        public string Program { get; set; }

        public string Twon { get; set; }
    }
}
