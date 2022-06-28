using System.Threading.Tasks;
using DotNetNow.API.Controllers.Base;
using DotNetNow.API.Helper;
using DotNetNow.Auth.UserService;
using DotNetNow.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DotNetNow.API.Controllers
{
    [Route("api/user", Name = "User")]
    [ApiController]
    public class UserController : BaseController
    {
        private IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;
        public UserController(IUserService userService,
            IConfiguration configuration,
             ILogger<UserController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet("cross-authenticate")]
        public async Task<string> CrossAuthenticate()
        {
            var crossAuthManager = new CrossAuthenticationManager(_configuration["IdentityServer:PrivateKey"], _configuration["IdentityServer:Url"]);
            var result = await crossAuthManager.Authenticate(UserLogin.Username);

            return result;
        }
    }
}
