using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using DotNetNow.Auth.Model;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNow.API.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        private CoreUser _userLogin;

        protected CoreUser UserLogin
        {
            get
            {
                _userLogin = new CoreUser()
                {
                    Id = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value,
                    FirstName = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value,
                    LastName = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value,
                    Email = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value,
                    Username = User?.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Username)?.Value
                };

                return _userLogin;
            }
        }

        protected string CurrentLang
        {
            get { return HttpContext.Request.Headers["Accept-Language"]; }
        }
    }
}