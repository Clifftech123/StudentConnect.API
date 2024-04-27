namespace StudentConnect.API.Models.Domain
{
    public class TokenInfo
    {
        public  Guid id   { get; set; }
        public string UuserName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public string? UserName { get; internal set; }
        public DateTime RefreshTokenExpiryTime { get; internal set; }
    }
}
