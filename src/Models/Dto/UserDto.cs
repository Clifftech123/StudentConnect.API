using System.Net.NetworkInformation;

namespace StudentConnect.API.src.Models.Dto
{
    public class UserDto
    {
        public  Guid guid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime LastLogin { get; set; }

        public Status Status { get; set; }
    }
}
