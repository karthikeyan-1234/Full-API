namespace Authenticate.Models
{
    public class TokenResponseModel
    {
        public TokenModel? JWTToken { get; set; }
        public TokenModel? RefreshToken { get; set; }
    }
}
