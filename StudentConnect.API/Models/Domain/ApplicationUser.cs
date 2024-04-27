using Microsoft.AspNetCore.Identity;

namespace StudentConnect.API.Models.Domain
{
    public class ApplicationUser : IdentityUser
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender  { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string School { get; set; }
        public string? Name { get; internal set; }
    }
}
