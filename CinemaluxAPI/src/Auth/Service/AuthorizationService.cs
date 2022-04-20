using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using CinemaluxAPI.Common.Enumerations;
using Microsoft.Extensions.Configuration;

namespace CinemaluxAPI.Auth
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Properties
        
        private IConfiguration appSettings;
        
        #endregion

        #region Constructor
        
        public AuthorizationService(IConfiguration configuration)
        {
            appSettings = configuration;
        }
        
        #endregion

        #region Action Methods

        public string GenerateJWT(Identity identity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.ASCII.GetBytes(
                identity.Role == 0 ? 
                    appSettings.GetSection("JWT").GetSection("UserSecret").Value :
                    appSettings.GetSection("JWT").GetSection("EmployeeSecret").Value 
                );
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", identity.Id.ToString()),
                    new Claim("name", identity.Name),
                    new Claim("surname", identity.Surname),
                    new Claim("username", identity.Username),
                    new Claim("email", identity.Email),
                    new Claim("contactPhone", identity.ContactPhone),
                    new Claim("role",  identity.Role.ToString()) 
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public Identity ValidateToken(string token, Role role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();            
            var key = Encoding.ASCII.GetBytes(
                role == Role.User ? 
                    appSettings.GetSection("JWT").GetSection("UserSecret").Value :
                    appSettings.GetSection("JWT").GetSection("EmployeeSecret").Value 
            );
           
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
                
            IEnumerable<Claim> payloadClaims = ((JwtSecurityToken) validatedToken).Payload.Claims;

            if (payloadClaims == null)
                throw new HttpResponseException(HttpStatusCode.Forbidden,"Token nije validan");
                
            // attach user to context on successful jwt validation
            return new Identity {
                Id = Int32.Parse(payloadClaims.First(x => x.Type == "id").Value),
                Name = payloadClaims.First(x => x.Type == "name").Value,
                Surname = payloadClaims.First(x => x.Type == "surname").Value,
                Username = payloadClaims.First(x => x.Type == "username").Value,
                Email = payloadClaims.First(x => x.Type == "email").Value,
                Role = Byte.Parse(payloadClaims.First(x => x.Type == "role").Value)
            };
        }

        #endregion
    }
}