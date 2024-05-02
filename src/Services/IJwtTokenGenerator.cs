using StudentConnect.API.src.Models.Data;

namespace StudentConnect.API.src.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
