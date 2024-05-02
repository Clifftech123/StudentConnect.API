using StudentConnect.API.src.Repositories;

namespace StudentConnect.API.src.Services
{
    public class UserService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator) : IUserService

    {
    }
}
