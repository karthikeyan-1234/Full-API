using Authenticate.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authenticate.Services
{
    public interface ITokenService
    {
        TokenModel GetJWTToken(IList<string> userRoles, IdentityUser user);
        TokenModel GetRefreshToken(IdentityUser user);
        bool IsValidJWT(string userName, string jwtToken);
        bool IsValidRefreshToken(string userName, string refreshToken);
    }
}