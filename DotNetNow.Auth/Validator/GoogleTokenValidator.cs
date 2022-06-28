using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotNetNow.Auth.Validator
{
    public static class GoogleTokenValidator
    {
        public static async Task<IList<Claim>> Validate(string token, string clientId)
        {
            var setting = new GoogleJsonWebSignature.ValidationSettings();
            var clientIDs = new List<string>();
            clientIDs.Add(clientId);
            setting.Audience = clientIDs;
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, setting);
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.NameId, payload.Name),
                    new Claim(JwtRegisteredClaimNames.FamilyName, payload.FamilyName),
                    new Claim(JwtRegisteredClaimNames.GivenName, payload.GivenName),
                    new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, payload.Subject),
                    new Claim(JwtRegisteredClaimNames.Iss, payload.Issuer),
                };

                return claims;
            } catch (Exception ex)
            {
                throw ex;
            }
             

         
        }
    }
}
