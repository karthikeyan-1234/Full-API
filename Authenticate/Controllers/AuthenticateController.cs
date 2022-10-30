﻿using Authenticate.Models;
using Authenticate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Authenticate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ILoginService _loginService;

        public AuthenticateController(IConfiguration configuration,ITokenService tokenService, ILoginService loginService,
            UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _loginService = loginService;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                var userRoles = await _userManager.GetRolesAsync(user);
                TokenResponseModel tokenResponse = new TokenResponseModel()
                {
                    JWTToken = _tokenService.GetJWTToken(userRoles, user),
                    RefreshToken = _tokenService.GetRefreshToken(user)
                };
                return Ok(tokenResponse);
            }

            return Unauthorized();
        }


        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest req)
        {
            if (_tokenService.IsValidJWT(req.userName,req.jwtToken))
            {
                if (_tokenService.IsValidRefreshToken(req.userName, req.refreshToken))
                {
                    var user = await _userManager.FindByNameAsync(req.userName);
                    var userRoles = await _userManager.GetRolesAsync(user);

                    TokenResponseModel tokenResponse = new TokenResponseModel()
                    {
                        JWTToken = _tokenService.GetJWTToken(userRoles, user),
                        RefreshToken = _tokenService.GetRefreshToken(user)
                    };
                    return Ok(tokenResponse);
                }

                return BadRequest("Invalid Refresh token");
            }
            return BadRequest("Invalid JWT");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var res = await _loginService.RegisterUserAsync(model);

            if (res?.StatusCode == StatusCodes.Status200OK)
                return Ok(res);
            else
                return StatusCode((int)res?.StatusCode,res.Object);
        }

        [HttpPost]
        [Route("register-operator")]
        public async Task<IActionResult> RegisterOperator([FromBody] RegisterModel model)
        {
            var res = await _loginService.RegisterOperatorAsync(model);
             
            if (res?.StatusCode == StatusCodes.Status200OK)
                return Ok(res);
            else
                return StatusCode((int)res?.StatusCode, res.Object);
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var res = await _loginService.RegisterUserAsync(model);

            if (res?.StatusCode == StatusCodes.Status200OK)
            {
                res = await _loginService.RegisterAdminAsync(model);

                if(res.StatusCode == StatusCodes.Status200OK)
                    return Ok(res);
            }

            return StatusCode((int)res?.StatusCode,res.Object);
        }


    }
}
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8604 // Possible null reference argument.

