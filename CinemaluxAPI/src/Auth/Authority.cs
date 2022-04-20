using System;
using System.Linq;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace CinemaluxAPI.Common
{
    public class Authority : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (string.IsNullOrEmpty(Roles))
            {
                context.HttpContext.Response.StatusCode = 500;
                context.HttpContext.Response.WriteAsync($"Role nisu navedene");
                return;
            }

            Identity identity = context.HttpContext.Items["Identity"] as Identity;
            string roleId = identity.Role.ToString();
            string roleName = Enum.Parse(typeof(Role), roleId).ToString();
            
            var requiredPermissions = Roles.Replace(" ","").Split(",");

            if (requiredPermissions.Contains(roleName))
                return;

            context.HttpContext.Response.StatusCode = 403;
            context.HttpContext.Response.WriteAsync($"{roleName} nema pristup ovoj ruti");
        }
    }
}