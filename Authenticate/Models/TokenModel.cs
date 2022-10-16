namespace Authenticate.Models
{
    public class TokenModel
    {
        public string? Token { get; set; }
        public string? UserName { get; set; }
        public DateTime Expires { get; set; }
        public bool is_expired { get; set; }
    }
}
