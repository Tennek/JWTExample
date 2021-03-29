using JWTSandbox.Authentication.API.Services.Audiences;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWTSandbox.Authentication.API.Issuer
{
    public interface ITokenManager
    {
        string GenerateToken(string userName, string audienceName, string audienceSecret);
        bool ValidateToken(string token);
        ClaimsPrincipal GetPrincipal(string token);
    }

    public class TokenManager : ITokenManager
    {
        private IConfiguration _config;
        private IAudienceService _audienceService;

        public TokenManager(IConfiguration config, IAudienceService audienceService)
        {
            _config = config;
            _audienceService = audienceService;
        }

        public string GenerateToken(string userName, string audienceName, string audienceSecret)
        {
            byte[] key = Convert.FromBase64String(audienceSecret);

            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _config["Authentication:Issuer"],

                Audience = audienceName,

                Subject = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, userName)
                }),

                Expires = DateTime.Now.AddMinutes(30),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };

            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);

            if (principal == null)
                return false;

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return false;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;

            return true;
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                var audience = _audienceService.FindAudienceByName(jwtToken.Audiences.FirstOrDefault());

                if (audience == null)
                    return null;

                byte[] key = Convert.FromBase64String(audience.AudienceSecret);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null; //todo
            }
        }
    }
}
