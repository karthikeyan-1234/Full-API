using Authenticate.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Authenticate.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public LoginService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<ResponseModel?> RegisterUserAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return new ResponseModel { Status = "Error", Message = "User already exists!", Object = userExists, StatusCode = StatusCodes.Status500InternalServerError };

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                TenantID = model.TenantID
            };
            var result = await _userManager.CreateAsync(user, model.Password); //Can also be created using only User. This is useful while using third party authentication providers
            if (!result.Succeeded)
                return new ResponseModel { Status = "Unable to create / register user", Message = JsonConvert.SerializeObject(result.Errors), Object = null, StatusCode = StatusCodes.Status500InternalServerError };

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(user, UserRoles.User);

            return new ResponseModel { Status = "Success", Message = "User created", Object = user, StatusCode = StatusCodes.Status200OK };
        }

        public async Task<ResponseModel?> RegisterOperatorAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return new ResponseModel { Status = "Error", Message = "User already exists!", Object = userExists, StatusCode = StatusCodes.Status500InternalServerError };

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                TenantID = model.TenantID
            };
            var result = await _userManager.CreateAsync(user, model.Password); //Can also be created using only User. This is useful while using third party authentication providers
            if (!result.Succeeded)
                return new ResponseModel { Status = "Unable to create / register user", Message = JsonConvert.SerializeObject(result.Errors), Object = null, StatusCode = StatusCodes.Status500InternalServerError };

            if (!await _roleManager.RoleExistsAsync(UserRoles.Operator))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Operator));

            if (await _roleManager.RoleExistsAsync(UserRoles.Operator))
                await _userManager.AddToRoleAsync(user, UserRoles.Operator);

            return new ResponseModel { Status = "Success", Message = "Operator created", Object = user, StatusCode = StatusCodes.Status200OK };
        }


        public async Task<ResponseModel> RegisterAdminAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return new ResponseModel { Status = "Error", Message = "User already exists!", StatusCode = StatusCodes.Status500InternalServerError };

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                TenantID = model.TenantID 
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new ResponseModel { Status = "Error", Message = "User creation failed! Please check user details and try again.", StatusCode = StatusCodes.Status500InternalServerError };

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            if (await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(user, UserRoles.User);

            return new ResponseModel { Status = "Success", Message = "Admin registered", Object = user, StatusCode = StatusCodes.Status200OK };
        }


        public async Task<ResponseModel> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user==null)
                return new ResponseModel { Status = "Error", Message = "User not found", Object = user, StatusCode = StatusCodes.Status500InternalServerError };

            if(await _userManager.CheckPasswordAsync(user,model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                TokenModel jwtToken = _tokenService.GetJWTToken(userRoles, user);
                TokenModel refreshToken = _tokenService.GetRefreshToken(user);
                TokenResponseModel tokenResponse = new TokenResponseModel { JWTToken = jwtToken, RefreshToken = refreshToken };
                return new ResponseModel { Status = "Success", Message = "Tokens created", Object = tokenResponse, StatusCode = StatusCodes.Status200OK };
            }

            return new ResponseModel { Status = "Error", Message = "Unable to Login", Object = null, StatusCode = StatusCodes.Status500InternalServerError };
        }

        public async Task<ResponseModel> VerifyLogin(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return new ResponseModel { Status = "Error", Message = "Unable to find user", StatusCode = StatusCodes.Status404NotFound };


            return new ResponseModel { Status = "Success", Message = "User logged in", StatusCode = StatusCodes.Status200OK };
        }
    }
}
