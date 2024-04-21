namespace StudentConnect.API.Models.Domain
{
    public class JWTSettings
    {
        public string Audience { get; set; }
        public string ValidIssuer { get; set; }
        public string SecretKey { get; set; }
    }
}
