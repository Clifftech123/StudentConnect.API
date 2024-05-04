using StudentConnect.API.src.Models.Data;

namespace StudentConnect.API.src.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
