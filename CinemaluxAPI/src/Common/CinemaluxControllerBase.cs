using CinemaluxAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaluxAPI.Common
{
    [ApiController]
    public class CinemaluxControllerBase : ControllerBase
    {
        private Identity _currentIdentity;
        public Identity CurrentIdentity => _currentIdentity ??= (Identity) HttpContext.Items["Identity"];
    }
}