namespace StudentConnect.API.src.Exceptions
{
    public class UserDeletedException : Exception
    {

        public UserDeletedException() { }

        public UserDeletedException(string message) : base(message) { }
    }
}
