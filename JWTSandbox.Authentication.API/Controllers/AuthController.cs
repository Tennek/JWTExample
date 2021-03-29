using JWTSandbox.Authentication.API.Issuer;
using JWTSandbox.Authentication.API.Models;
using JWTSandbox.Authentication.API.Services.Audiences;
using JWTSandbox.Authentication.API.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTSandbox.Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ITokenManager _tokenManager;
        private IAudienceService _audienceService;
        private IUserService _userService;

        //https://www.c-sharpcorner.com/blogs/jwt-based-tokenisation-via-net-core
        //https://www.red-gate.com/simple-talk/dotnet/net-development/jwt-authentication-microservices-net/

        public AuthController(ITokenManager tokenManager, IAudienceService audienceService, IUserService userService)
        {
            _tokenManager = tokenManager;
            _audienceService = audienceService;
            _userService = userService;
        }

        //https://localhost:44318/api/auth/Login
        //{
        // UserName : "Test1",
        // Password : "Test1",
        // AudienceId : "099153c2625149bc8ecb3e85e03f0022"
        //}
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid request");
            }

            //check audience
            var audience = _audienceService.FindAudienceById(user.AudienceId);
            if(audience == null)
            {
                return Unauthorized();
            }

            //check user
            if (_userService.UserExist(user.UserName, user.Password, user.AudienceId))
            {
                return Ok( new { Token = _tokenManager.GenerateToken(user.UserName, audience.Name, audience.AudienceSecret)});
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet, Route("validate")]
        public async Task<IActionResult> Validate(string token)
        {
            var isValid = _tokenManager.ValidateToken(token);

            if (isValid)
                return Ok();

            return Unauthorized();
        }
    }
}
