using System.ComponentModel.DataAnnotations;

namespace DotNetNow.Auth.UserService
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
