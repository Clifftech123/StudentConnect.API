namespace StudentConnect.API.Models.Domain
{
    public class TokenInfo
    {
        public  Guid id   { get; set; }
        public string Usename { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
