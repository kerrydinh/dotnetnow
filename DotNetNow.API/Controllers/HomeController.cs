using DotNetNow.API.Controllers.Base;
using DotNetNow.Auth.UserService;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNow.API.Controllers
{
    [ApiController]
    [Route("api/home", Name = "home")]
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
    }
}