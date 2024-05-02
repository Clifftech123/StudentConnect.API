namespace StudentConnect.API.src.Exceptions
{
    public class UserBannedException : Exception
    {
        public UserBannedException() { }

        public UserBannedException(string message) : base(message) { }
    }
}
