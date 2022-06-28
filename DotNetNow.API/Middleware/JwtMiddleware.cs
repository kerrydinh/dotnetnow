using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using DotNetNow.Auth.Model;
using DotNetNow.Auth.Validator;
using DotNetNow.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DotNetNow.API.Middleware
{

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public JwtMiddleware(RequestDelegate next,
            IConfiguration configuration)
        {
            _next = next;
            _tokenHandler = new JwtSecurityTokenHandler();
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, UserManager<AppUser> userManager)
        {
            context.Request.Headers.TryGetValue("Authorization", out var tokenValues);
            var token = tokenValues.FirstOrDefault()?.Replace("Bearer ", string.Empty);
            if (!_tokenHandler.CanReadToken(token))
                await _next(context);

            context.Request.Headers.TryGetValue("Provider", out var provider);
            switch (provider.FirstOrDefault()?.ToLower())
            {
                case ProviderTypes.Core:
                    await HandleCoreAuthentication(context, userManager, token);
                    break;
                case ProviderTypes.Google:
                    await HandleGoogleAuthentication(context, userManager, token);
                    break;
                default:
                    break;
            }

            await _next(context);
        }

        private async Task HandleCoreAuthentication(HttpContext context, UserManager<AppUser> userManager, string token)
        {
            if (token != null)
            {
                var myIssuer = _configuration["ClientAppUrl"];
                var myAudience = _configuration["ClientAppUrl"];
                var secretKey = _configuration["AuthSetting:SecretKey"];
                var validatedTokenClaims = JwtValidator.Validate(token, myIssuer, myAudience, secretKey);
                var userId = validatedTokenClaims.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value;
                context.Items["User"] = await userManager.FindByIdAsync(userId);
            }
        }

        private async Task HandleGoogleAuthentication(HttpContext context, UserManager<AppUser> userManager, string token)
        {
            if (token != null)
            {
                var clientId = _configuration["GoogleAPI:ClientId"];
                var googleClaims = await GoogleTokenValidator.Validate(token, clientId);
                var email = googleClaims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                context.Items["User"] = await userManager.FindByEmailAsync(email);
            }
        }
    }
}
