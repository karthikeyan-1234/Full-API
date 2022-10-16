namespace Authenticate.Models
{
    public class RefreshTokenRequest
    {
        public string ?userName { get; set; }
        public string ?refreshToken { get; set; } 
        public string ?jwtToken { get; set; }
    }
}
