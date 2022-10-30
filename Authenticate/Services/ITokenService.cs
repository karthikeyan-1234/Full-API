using Authenticate.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authenticate.Services
{
    public interface ITokenService
    {
        TokenModel GetJWTToken(IList<string> userRoles, ApplicationUser user);
        TokenModel GetRefreshToken(ApplicationUser user);
        bool IsValidJWT(string userName, string jwtToken);
        bool IsValidRefreshToken(string userName, string refreshToken);
    }
}