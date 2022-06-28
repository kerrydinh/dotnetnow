using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotNetNow.Auth.Builder;
using DotNetNow.Auth.Model;
using DotNetNow.Domain.Entity;

namespace DotNetNow.Auth.UserService
{
    public class UserService : IUserService
    {
        private AppUser _currentUser;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public void SetUser(AppUser user)
        {
            _currentUser = user;
        }

        public AppUser GetUser()
        {
            return _currentUser;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, lockoutOnFailure: false);

            if (!result.Succeeded)
                return null;

            var user = await _userManager.FindByEmailAsync(model.Username);
            var claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()));
            claims.Add(new Claim(CustomClaimTypes.Username, user.UserName.ToString()));
            claims.Add(new Claim(CustomClaimTypes.Provider, ProviderTypes.Core));

            var myIssuer = _configuration["ClientAppUrl"];
            var myAudience = _configuration["ClientAppUrl"];
            var key = Encoding.ASCII.GetBytes(_configuration["AuthSetting:SecretKey"]);
            var token = JwtBuilder.BuildToken(claims, myIssuer, myAudience, key, DateTime.UtcNow.AddDays(7));
            return new AuthenticateResponse(user, token);
        }

    }
}
