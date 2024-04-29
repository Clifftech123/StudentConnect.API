namespace StudentConnect.API.Models.Domain
{
    public static class StatusMessages
    {
        public const string MessageInvalidFields = "Please pass all the required fields";
        public const string MessageUserExists = "User already exists!";
        public const string MessageUserCreationFailed = "User creation failed! Please check user details and try again.";
        public const string MessageRegistrationSuccess = "Successfully registered";
        public const string MessageInvalidCredentials = "Invalid Username or Password";
        public const string MessageLoginSuccess = "Logged in";
        public const string MessageServerError = "An error occurred while processing your request";
        public const string MessageUserNotFound = "User not found";
        public const string MessageInvalidCurrentPassword = "Invalid current password";
        public const string MessagePasswordChangeFailed = "Password change failed";
        public const string MessagePasswordChangedSuccessfully = "Password changed successfully";
        public const int StatusCodeSuccess = 1;
    }

}
