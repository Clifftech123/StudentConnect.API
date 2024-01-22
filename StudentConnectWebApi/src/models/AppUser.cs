using Microsoft.AspNetCore.Identity;

namespace StudentConnectWebApi.models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string School { get; set; }
        public string Grade { get; set; } // New property
    }
}