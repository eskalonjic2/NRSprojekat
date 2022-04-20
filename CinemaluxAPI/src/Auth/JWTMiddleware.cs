using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace CinemaluxAPI.Auth
{
    public class JWTMiddleware
    {
        
        #region Properties
        
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;
        //TODO Fix this garbage
        private readonly string[] _whitelistedRoutes = new string[] { "auth", "api", "swagger", "web", "multimedia" };
            
        #endregion
        
        #region Constructor
        
        public JWTMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _configuration = configuration;
            _next = next;
        }
        
        #endregion
        
        public async Task Invoke(HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");

            var token = header?.Last();
            var role = header?[0];

            var paths = context.Request.Path.ToString().Replace("/api/","/").Split("/");
            var targetedController = paths[1];
            
            if (_whitelistedRoutes.Contains(targetedController) || paths.Length > 2 && paths[2] == "web")
                await _next(context);
            else
            {
                if (token != null)
                {
                    AttachUserToContext(context, token, role);
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync("Token ne postoji");
                }   
            }
        }

        private void AttachUserToContext(HttpContext context, string token, string role)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();            
                var key = Encoding.ASCII.GetBytes(
                    role.Equals("User") ? 
                        _configuration.GetSection("JWT").GetSection("UserSecret").Value :
                        _configuration.GetSection("JWT").GetSection("EmployeeSecret").Value
                    );
             
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                
                //TODO Fix this if possible
                IEnumerable<Claim> payloadClaims = ((JwtSecurityToken) validatedToken).Payload.Claims;

                if (payloadClaims == null)
                    return;
              
                // attach user to context on successful jwt validation
                context.Items["Identity"] = new Identity
                {
                    Id = Int32.Parse(payloadClaims.First(x => x.Type == "id").Value),
                    Name = payloadClaims.First(x => x.Type == "name").Value,
                    Surname = payloadClaims.First(x => x.Type == "surname").Value,
                    Username = payloadClaims.First(x => x.Type == "username").Value,
                    Role = Byte.Parse(payloadClaims.First(x => x.Type == "role").Value)
                };
            }
            catch
            {
                context.Response.StatusCode = 401;
                context.Response.WriteAsJsonAsync("Token istekao ili nije validan");
            }

        }
    }
}
