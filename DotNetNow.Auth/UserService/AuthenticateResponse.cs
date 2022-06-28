using DotNetNow.Domain.Entity;

namespace DotNetNow.Auth.UserService
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(AppUser user, string token)
        {
            Id = user.Id;
            FirstName = null;
            LastName = null;
            Username = user.UserName;
            Token = token;
        }
    }
}