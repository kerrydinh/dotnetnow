using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using DotNetNow.Domain.Entity;

namespace DotNetNow.Application.Main
{
    public class BaseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public BaseService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
    }
}
