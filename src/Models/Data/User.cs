using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;

namespace StudentConnect.API.src.Models.Data
{
    public class User: IdentityUser
    {
        public DateTime LastLogin { get; set; }

        public Status Status { get; set; } 
    }
}
