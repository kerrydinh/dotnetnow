using Newtonsoft.Json;

namespace DotNetNow.Auth.Model
{
    public class CoreUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore] public string Password { get; set; }
    }
}