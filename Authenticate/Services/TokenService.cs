using Authenticate.Contexts;
using Authenticate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Authenticate.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenDBContext db;

        public TokenService(IConfiguration configuration,UserManager<ApplicationUser> userManager,TokenDBContext db)
        {
            _configuration = configuration;
            _userManager = userManager;
            this.db = db;
        }

        public TokenModel GetJWTToken(IList<string> userRoles,ApplicationUser user)
        {

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("TenantID",user.TenantID)
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var jwttoken = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddSeconds(15),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new TokenModel() {
                UserName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwttoken),
                Expires = jwttoken.ValidTo,
                is_expired = false
            };

        }
        public TokenModel GetRefreshToken(ApplicationUser user)
        {
            var rand_no = new byte[32];
            var rand_gen = RandomNumberGenerator.Create();
            rand_gen.GetBytes(rand_no);
            var ref_token = Convert.ToBase64String(rand_no);

            var result = db?.Tokens?.Where(t => t.is_expired == false && t.UserName == user.UserName);

            foreach (var _token in result)
                _token.is_expired = true;

            db.SaveChanges();

            TokenModel token = new TokenModel
            {
                Token = ref_token,
                Expires = DateTime.Now.AddSeconds(30),
                UserName = user?.UserName,
                is_expired = false
            };

            db.Tokens.Add(token);
            db.SaveChanges();
            return token;
        }
        public bool IsValidJWT(string userName, string jwtToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidateLifetime = false
            };
            ClaimsPrincipal principal = handler.ValidateToken(jwtToken, validationParameters, out SecurityToken validToken);

            if (principal.FindFirst(ClaimTypes.Name).Value == userName && (validToken as JwtSecurityToken).Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                return true;

            return false;
        }
        public bool IsValidRefreshToken(string userName, string refreshToken)
        {
            var current_time = DateTime.Now;
            var refToken = db.Tokens?.Where(t => t.Token == refreshToken && t.UserName == userName &&
            t.is_expired == false && t.Expires >= current_time).FirstOrDefault();

            if (refToken != null)
                return true;

            return false;
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.

